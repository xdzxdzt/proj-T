using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Application.Reports;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class LessonPackagesService : ILessonPackagesService
    {
        private readonly ILessonPackagesRepository _lessonPackagesRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly ITutorSubjectsRepository _tutorSubjectsRepository;
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IUsersRepository _usersRepository;

        public LessonPackagesService(
            ILessonPackagesRepository lessonPackagesRepository,
            IStudentsRepository studentsRepository,
            ITutorsRepository tutorsRepository,
            ITutorSubjectsRepository tutorSubjectsRepository,
            ISubjectsRepository subjectsRepository,
            IUsersRepository usersRepository)
        {
            _lessonPackagesRepository = lessonPackagesRepository;
            _studentsRepository = studentsRepository;
            _tutorsRepository = tutorsRepository;
            _tutorSubjectsRepository = tutorSubjectsRepository;
            _subjectsRepository = subjectsRepository;
            _usersRepository = usersRepository;
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

        public async Task<List<LessonPackageInfo>> GetForStudent(Guid userId)
        {
            var student = await _studentsRepository.GetByUserId(userId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var lessonPackages = await _lessonPackagesRepository.GetByStudentId(student.Id);
            return await BuildLessonPackageInfos(lessonPackages);
        }

        public async Task<List<LessonPackageInfo>> GetForTutor(Guid userId)
        {
            var tutor = await _tutorsRepository.GetByUserId(userId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            var lessonPackages = await _lessonPackagesRepository.GetByTutorId(tutor.Id);
            return await BuildLessonPackageInfos(lessonPackages);
        }

        private async Task<List<LessonPackageInfo>> BuildLessonPackageInfos(List<LessonPackage> lessonPackages)
        {
            var result = new List<LessonPackageInfo>();

            foreach (var lessonPackage in lessonPackages)
            {
                var student = await _studentsRepository.GetById(lessonPackage.StudentId);

                if (student is null)
                {
                    throw new InvalidOperationException("Обучающийся не найден");
                }

                var studentUser = await _usersRepository.GetById(student.UserId);

                if (studentUser is null)
                {
                    throw new InvalidOperationException("Пользователь обучающегося не найден");
                }

                var tutorSubject = await _tutorSubjectsRepository.GetById(lessonPackage.TutorSubjectId);

                if (tutorSubject is null)
                {
                    throw new InvalidOperationException("Предмет репетитора не найден");
                }

                var tutor = await _tutorsRepository.GetById(tutorSubject.TutorId);

                if (tutor is null)
                {
                    throw new InvalidOperationException("Репетитор не найден");
                }

                var tutorUser = await _usersRepository.GetById(tutor.UserId);

                if (tutorUser is null)
                {
                    throw new InvalidOperationException("Пользователь репетитора не найден");
                }

                var subject = await _subjectsRepository.GetById(tutorSubject.SubjectId);

                if (subject is null)
                {
                    throw new InvalidOperationException("Предмет не найден");
                }

                result.Add(new LessonPackageInfo(
                    lessonPackage.Id,
                    student.Id,
                    studentUser.Id,
                    studentUser.UserName,
                    studentUser.FirstName,
                    studentUser.LastName,
                    tutorSubject.Id,
                    tutor.Id,
                    tutorUser.Id,
                    tutorUser.UserName,
                    tutorUser.FirstName,
                    tutorUser.LastName,
                    subject.Id,
                    subject.Name,
                    lessonPackage.TotalLessons,
                    lessonPackage.RemainingLessons,
                    lessonPackage.PricePerLesson,
                    lessonPackage.TotalPrice,
                    lessonPackage.PurchasedAt,
                    lessonPackage.CompletedAt));
            }

            return result
                .OrderByDescending(x => x.PurchasedAt)
                .ToList();
        }
    }
}
