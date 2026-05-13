using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class TutorSubjectService : ITutorSubjectService
    {
        private readonly ITutorSubjectsRepository _tutorSubjectRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly ISubjectsRepository _subjectsRepository;

        public TutorSubjectService(
            ITutorSubjectsRepository tutorSubjectRepository,
            ITutorsRepository tutorsRepository,
            ISubjectsRepository subjectsRepository)
        {
            _tutorSubjectRepository = tutorSubjectRepository;
            _tutorsRepository = tutorsRepository;
            _subjectsRepository = subjectsRepository;
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
    }
}
