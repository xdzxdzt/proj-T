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
        private readonly ILessonPackagesRepository _lessonPackagesRepository;

        public SolutionsService(
            ISolutionsRepository solutionsRepository,
            ITutorTasksRepository tutorTasksRepository,
            IStudentsRepository studentsRepository,
            ILessonPackagesRepository lessonPackagesRepository)
        {
            _solutionsRepository = solutionsRepository;
            _tutorTasksRepository = tutorTasksRepository;
            _studentsRepository = studentsRepository;
            _lessonPackagesRepository = lessonPackagesRepository;
        }

        public async Task SubmitSolution(Guid studentId, Guid tutorTaskId, string answerText)
        {
            var tutorTask = await _tutorTasksRepository.GetById(tutorTaskId);

            if (tutorTask is null)
            {
                throw new InvalidOperationException("Задание репетитора не найдено");
            }

            if(tutorTask.ClosedAt is not null)
            {
                throw new InvalidOperationException("Задание уже закрыто");
            }

            var student = await _studentsRepository.GetById(studentId);

            if(student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
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

            if(alreadyExists)
            {
                throw new InvalidOperationException("Обучающийся уже отправил решение");
            }

            var solution = Solution.Create(
                Guid.NewGuid(), 
                tutorTask.Id, 
                student.Id, 
                answerText);

            if(solution.IsFailure)
            {
                throw new InvalidOperationException(solution.Error);
            }

            await _solutionsRepository.Add(solution.Value);
        }
    }
}
