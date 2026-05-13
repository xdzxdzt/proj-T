using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class TutorTasksService : ITutorTasksService
    {
        private readonly ITutorTasksRepository _tutorTasksRepository;
        private readonly ILessonPackagesRepository _lessonPackagesRepository;
        private readonly ITutorSubjectsRepository _tutorSubjectsRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly IStudentsRepository _studentsRepository;

        public TutorTasksService(
            ITutorTasksRepository tutorTasksRepository,
            ILessonPackagesRepository lessonPackagesRepository,
            ITutorSubjectsRepository tutorSubjectsRepository,
            ITutorsRepository tutorsRepository,
            IStudentsRepository studentsRepository)
        {
            _tutorTasksRepository = tutorTasksRepository;
            _lessonPackagesRepository = lessonPackagesRepository;
            _tutorSubjectsRepository = tutorSubjectsRepository;
            _tutorsRepository = tutorsRepository;
            _studentsRepository = studentsRepository;
        }

        public async Task CreateTask(Guid userId, Guid lessonPackageId, string title, string description)
        {
            var tutor = await GetTutorByUserId(userId);
            var lessonPackage = await GetTutorLessonPackage(tutor.Id, lessonPackageId);

            var tutorTask = TutorTask.Create(
                Guid.NewGuid(),
                lessonPackage.Id,
                title,
                description);

            if (tutorTask.IsFailure)
            {
                throw new InvalidOperationException(tutorTask.Error);
            }

            await _tutorTasksRepository.Add(tutorTask.Value);
        }

        public async Task<List<TutorTask>> GetByLessonPackageId(Guid userId, Guid lessonPackageId)
        {
            var tutor = await GetTutorByUserId(userId);
            await GetTutorLessonPackage(tutor.Id, lessonPackageId);

            return await _tutorTasksRepository.GetByLessonPackageId(lessonPackageId);
        }

        public async Task<List<TutorTask>> GetForStudent(Guid userId)
        {
            var student = await _studentsRepository.GetByUserId(userId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var lessonPackages = await _lessonPackagesRepository.GetByStudentId(student.Id);
            var tasks = new List<TutorTask>();

            foreach (var lessonPackage in lessonPackages)
            {
                var packageTasks = await _tutorTasksRepository.GetByLessonPackageId(lessonPackage.Id);
                tasks.AddRange(packageTasks);
            }

            return tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToList();
        }

        public async Task CloseTask(Guid userId, Guid taskId)
        {
            var tutor = await GetTutorByUserId(userId);
            var tutorTask = await _tutorTasksRepository.GetById(taskId);

            if (tutorTask is null)
            {
                throw new InvalidOperationException("Задание не найдено");
            }

            await GetTutorLessonPackage(tutor.Id, tutorTask.LessonPackageId);

            var result = tutorTask.Close();

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            await _tutorTasksRepository.Update(tutorTask);
        }

        private async Task<Tutor> GetTutorByUserId(Guid userId)
        {
            var tutor = await _tutorsRepository.GetByUserId(userId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            return tutor;
        }

        private async Task<LessonPackage> GetTutorLessonPackage(Guid tutorId, Guid lessonPackageId)
        {
            var lessonPackage = await _lessonPackagesRepository.GetById(lessonPackageId);

            if (lessonPackage is null)
            {
                throw new InvalidOperationException("Пакет занятий не найден");
            }

            var tutorSubject = await _tutorSubjectsRepository.GetById(lessonPackage.TutorSubjectId);

            if (tutorSubject is null)
            {
                throw new InvalidOperationException("Предмет репетитора не найден");
            }

            if (tutorSubject.TutorId != tutorId)
            {
                throw new InvalidOperationException("Репетитор не может работать с чужим пакетом занятий");
            }

            return lessonPackage;
        }
    }
}
