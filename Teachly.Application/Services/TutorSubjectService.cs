using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Application.Reports;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class TutorSubjectService : ITutorSubjectService
    {
        private readonly ITutorSubjectsRepository _tutorSubjectRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly ISubjectsRepository _subjectsRepository;
        private readonly IUsersRepository _usersRepository;

        public TutorSubjectService(
            ITutorSubjectsRepository tutorSubjectRepository,
            ITutorsRepository tutorsRepository,
            ISubjectsRepository subjectsRepository,
            IUsersRepository usersRepository)
        {
            _tutorSubjectRepository = tutorSubjectRepository;
            _tutorsRepository = tutorsRepository;
            _subjectsRepository = subjectsRepository;
            _usersRepository = usersRepository;
        }

        public async Task AddSubjectToTutor(Guid userId, Guid subjectId, decimal pricePerLesson)
        {
            var tutor = await _tutorsRepository.GetByUserId(userId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            var subject = await _subjectsRepository.GetById(subjectId);

            if (subject is null)
            {
                throw new InvalidOperationException("Предмет не существует");
            }

            var alreadyExists = await _tutorSubjectRepository.Exists(tutor.Id, subject.Id);

            if (alreadyExists)
            {
                throw new InvalidOperationException("Репетитор уже ведет этот предмет");
            }

            var subjectTutor = TutorSubject.Create(
                Guid.NewGuid(),
                tutor.Id,
                subject.Id,
                pricePerLesson);

            if (subjectTutor.IsFailure)
            {
                throw new InvalidOperationException(subjectTutor.Error);
            }

            await _tutorSubjectRepository.Add(subjectTutor.Value);
        }

        public async Task<List<TutorSubjectInfo>> GetAll()
        {
            var tutorSubjects = await _tutorSubjectRepository.GetAll();
            return await BuildTutorSubjectInfos(tutorSubjects);
        }

        public async Task<List<TutorSubjectInfo>> GetByTutorId(Guid tutorId)
        {
            var tutor = await _tutorsRepository.GetById(tutorId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            var tutorSubjects = await _tutorSubjectRepository.GetAllByTutorId(tutor.Id);
            return await BuildTutorSubjectInfos(tutorSubjects);
        }

        private async Task<List<TutorSubjectInfo>> BuildTutorSubjectInfos(List<TutorSubject> tutorSubjects)
        {
            var result = new List<TutorSubjectInfo>();

            foreach (var tutorSubject in tutorSubjects)
            {
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

                result.Add(new TutorSubjectInfo(
                    tutorSubject.Id,
                    tutor.Id,
                    tutorUser.Id,
                    tutorUser.UserName,
                    tutorUser.FirstName,
                    tutorUser.LastName,
                    tutorUser.AvatarUrl,
                    tutor.Description,
                    tutor.AverageRating,
                    tutor.RatingCount,
                    subject.Id,
                    subject.Name,
                    tutorSubject.PricePerLesson));
            }

            return result
                .OrderBy(x => x.SubjectName)
                .ThenBy(x => x.TutorLastName)
                .ThenBy(x => x.TutorFirstName)
                .ToList();
        }
    }
}
