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

        public TutorTasksService(
            ITutorTasksRepository tutorTasksRepository, 
            ILessonPackagesRepository lessonPackagesRepository,
            ITutorSubjectsRepository tutorSubjectsRepository)
        {
            _tutorTasksRepository = tutorTasksRepository;
            _lessonPackagesRepository = lessonPackagesRepository;
            _tutorSubjectsRepository = tutorSubjectsRepository;
        }

        public async Task CreateTask(Guid tutorId, Guid lessonPackageId, string title, string description)
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
                throw new InvalidOperationException("Репетитор не может создать задание в чужом пакете занятий");
            }

            var tutorTask = TutorTask.Create(Guid.NewGuid(),
                lessonPackage.Id,
                title,
                description);

            if(tutorTask.IsFailure)
            {
                throw new InvalidOperationException(tutorTask.Error);
            }

            await _tutorTasksRepository.Add(tutorTask.Value);
        }

        public async Task CloseTask(Guid tutorId, Guid taskId)
        {
            var tutorTask = await _tutorTasksRepository.GetById(taskId);

            if (tutorTask is null)
            {
                throw new InvalidOperationException("Задание не найдено");
            }

            var lessonPackage = await _lessonPackagesRepository.GetById(tutorTask.LessonPackageId);

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
                throw new InvalidOperationException("Репетитор не может закрыть чужое задание");
            }

            var result = tutorTask.Close();

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            await _tutorTasksRepository.Update(tutorTask);
        }
    }
}
