using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class LessonPackagesService : ILessonPackagesService
    {
        private readonly ILessonPackagesRepository _lessonPackagesRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly ITutorSubjectsRepository _tutorSubjectsRepository;

        public LessonPackagesService(
            ILessonPackagesRepository lessonPackagesRepository,
            IStudentsRepository studentsRepository,
            ITutorsRepository tutorsRepository,
            ITutorSubjectsRepository tutorSubjectsRepository)
        {
            _lessonPackagesRepository = lessonPackagesRepository;
            _studentsRepository = studentsRepository;
            _tutorsRepository = tutorsRepository;
            _tutorSubjectsRepository = tutorSubjectsRepository;
        }

        public async Task BuyPackage(
            Guid userId,
            Guid tutorSubjectId,
            int totalLessons)
        {
            var student = await _studentsRepository.GetByUserId(userId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var tutorSubject = await _tutorSubjectsRepository.GetById(tutorSubjectId);

            if (tutorSubject is null)
            {
                throw new InvalidOperationException("Предмет не найден");
            }

            var lessonPackage = LessonPackage.Create(
                Guid.NewGuid(),
                student.Id,
                tutorSubject.Id,
                totalLessons,
                tutorSubject.PricePerLesson);

            if (lessonPackage.IsFailure)
            {
                throw new InvalidOperationException(lessonPackage.Error);
            }

            await _lessonPackagesRepository.Add(lessonPackage.Value);
        }

        public async Task UseLesson(Guid userId, Guid lessonPackageId)
        {
            var tutor = await _tutorsRepository.GetByUserId(userId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

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

            if (tutorSubject.TutorId != tutor.Id)
            {
                throw new InvalidOperationException("Репетитор не может использовать занятие из чужого пакета");
            }

            var result = lessonPackage.UseLesson();

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            await _lessonPackagesRepository.Update(lessonPackage);
        }
    }
}
