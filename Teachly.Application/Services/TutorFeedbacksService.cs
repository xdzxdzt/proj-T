using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class TutorFeedbacksService : ITutorFeedbacksService
    {
        private readonly ITutorFeedbacksRepository _tutorFeedbacksRepository;
        private readonly ISolutionsRepository _solutionsRepository;
        private readonly ITutorTasksRepository _tutorTasksRepository;
        private readonly ILessonPackagesRepository _lessonPackagesRepository;
        private readonly ITutorSubjectsRepository _tutorSubjectsRepository;

        public TutorFeedbacksService(
            ITutorFeedbacksRepository tutorFeedbacksRepository,
            ISolutionsRepository solutionsRepository,
            ITutorTasksRepository tutorTasksRepository,
            ILessonPackagesRepository lessonPackagesRepository,
            ITutorSubjectsRepository tutorSubjectsRepository)
        {
            _tutorFeedbacksRepository = tutorFeedbacksRepository;
            _solutionsRepository = solutionsRepository;
            _tutorTasksRepository = tutorTasksRepository;
            _lessonPackagesRepository = lessonPackagesRepository;
            _tutorSubjectsRepository = tutorSubjectsRepository;
        }

        public async Task GiveFeedback(Guid tutorId, Guid solutionId, short grade, string tutorComment)
        {
            var solution = await _solutionsRepository.GetById(solutionId);

            if (solution is null)
            {
                throw new InvalidOperationException("Решение не найдено");
            }

            var tutorTask = await _tutorTasksRepository.GetById(solution.TutorTaskId);

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
                throw new InvalidOperationException("Репетитор не может проверить чужое решение");
            }

            var alreadyExists = await _tutorFeedbacksRepository.ExistsBySolutionId(solution.Id);

            if (alreadyExists)
            {
                throw new InvalidOperationException("Решение уже проверено");
            }

            var feedback = TutorFeedback.Create(
                Guid.NewGuid(), 
                solution.Id, 
                tutorComment, 
                grade);

            if(feedback.IsFailure)
            {
                throw new InvalidOperationException(feedback.Error);
            }

            await _tutorFeedbacksRepository.Add(feedback.Value);
        }
    }
}
