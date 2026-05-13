using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class SolutionsService : ISolutionsService
    {
        private readonly ISolutionsRepository _solutionsRepository;
        private readonly ITutorTasksRepository _tutorTasksRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly ILessonPackagesRepository _lessonPackagesRepository;
        private readonly ITutorFeedbacksRepository _tutorFeedbacksRepository;

        public SolutionsService(
            ISolutionsRepository solutionsRepository,
            ITutorTasksRepository tutorTasksRepository,
            IStudentsRepository studentsRepository,
            ITutorsRepository tutorsRepository,
            ILessonPackagesRepository lessonPackagesRepository,
            ITutorFeedbacksRepository tutorFeedbacksRepository)
        {
            _solutionsRepository = solutionsRepository;
            _tutorTasksRepository = tutorTasksRepository;
            _studentsRepository = studentsRepository;
            _tutorsRepository = tutorsRepository;
            _lessonPackagesRepository = lessonPackagesRepository;
            _tutorFeedbacksRepository = tutorFeedbacksRepository;
        }

        public async Task SubmitSolution(Guid userId, Guid tutorTaskId, string answerText)
        {
            var student = await _studentsRepository.GetByUserId(userId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var tutorTask = await _tutorTasksRepository.GetById(tutorTaskId);

            if (tutorTask is null)
            {
                throw new InvalidOperationException("Задание репетитора не найдено");
            }

            if (tutorTask.ClosedAt is not null)
            {
                throw new InvalidOperationException("Задание уже закрыто");
            }

            var lessonPackage = await _lessonPackagesRepository.GetById(tutorTask.LessonPackageId);

            if (lessonPackage is null)
            {
                throw new InvalidOperationException("Пакет занятий не найден");
            }

            if (lessonPackage.StudentId != student.Id)
            {
                throw new InvalidOperationException("Обучающийся не может отправить решение по чужому заданию");
            }

            var alreadyExists = await _solutionsRepository.ExistsByTutorTaskIdAndStudentId(tutorTask.Id, student.Id);

            if (alreadyExists)
            {
                throw new InvalidOperationException("Обучающийся уже отправил решение");
            }

            var solution = Solution.Create(
                Guid.NewGuid(),
                tutorTask.Id,
                student.Id,
                answerText);

            if (solution.IsFailure)
            {
                throw new InvalidOperationException(solution.Error);
            }

            await _solutionsRepository.Add(solution.Value);
        }

        public async Task<List<Solution>> GetAllByStudentId(Guid userId, Guid studentId)
        {
            var tutor = await _tutorsRepository.GetByUserId(userId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            var student = await _studentsRepository.GetById(studentId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var tutorLessonPackages = await _lessonPackagesRepository.GetByTutorId(tutor.Id);
            var hasAccessToStudent = tutorLessonPackages.Any(x => x.StudentId == student.Id);

            if (!hasAccessToStudent)
            {
                throw new InvalidOperationException("Репетитор не может просматривать решения этого обучающегося");
            }

            return await _solutionsRepository.GetAllByStudentId(student.Id);
        }

        public async Task<List<(Solution Solution, TutorTask TutorTask, TutorFeedback? TutorFeedback)>> GetHistoryForStudent(Guid userId)
        {
            var student = await _studentsRepository.GetByUserId(userId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var solutions = await _solutionsRepository.GetAllByStudentId(student.Id);
            var history = new List<(Solution Solution, TutorTask TutorTask, TutorFeedback? TutorFeedback)>();

            foreach (var solution in solutions)
            {
                var tutorTask = await _tutorTasksRepository.GetById(solution.TutorTaskId);

                if (tutorTask is null)
                {
                    throw new InvalidOperationException("Задание не найдено");
                }

                var tutorFeedback = await _tutorFeedbacksRepository.GetBySolutionId(solution.Id);
                history.Add((solution, tutorTask, tutorFeedback));
            }

            return history
                .OrderByDescending(x => x.Solution.SubmittedAt)
                .ToList();
        }
    }
}
