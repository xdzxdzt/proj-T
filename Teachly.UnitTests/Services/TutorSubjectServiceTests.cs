using Moq;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class TutorSubjectServiceTests
    {
        [Test]
        public async Task AddSubjectToTutorWithValidDataAddsTutorSubject()
        {
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();
            var subjectsRepository = new Mock<ISubjectsRepository>();

            var userId = Guid.NewGuid();
            var tutor = TestData.Tutor(userId: userId);
            var subject = TestData.Subject();

            tutorsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(tutor);
            subjectsRepository.Setup(x => x.GetById(subject.Id)).ReturnsAsync(subject);
            tutorSubjectsRepository.Setup(x => x.Exists(tutor.Id, subject.Id)).ReturnsAsync(false);
            var service = new TutorSubjectService(
                tutorSubjectsRepository.Object,
                tutorsRepository.Object,
                subjectsRepository.Object,
                new Mock<IUsersRepository>().Object);

            await service.AddSubjectToTutor(userId, subject.Id, 500m);

            tutorSubjectsRepository.Verify(x => x.Add(It.Is<TutorSubject>(ts =>
                ts.TutorId == tutor.Id &&
                ts.SubjectId == subject.Id &&
                ts.PricePerLesson == 500m)), Times.Once);
        }

        [Test]
        public void AddSubjectToTutorWhenAlreadyExistsThrowsInvalidOperationException()
        {
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();
            var subjectsRepository = new Mock<ISubjectsRepository>();

            var userId = Guid.NewGuid();
            var tutor = TestData.Tutor(userId: userId);
            var subject = TestData.Subject();

            tutorsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(tutor);
            subjectsRepository.Setup(x => x.GetById(subject.Id)).ReturnsAsync(subject);
            tutorSubjectsRepository.Setup(x => x.Exists(tutor.Id, subject.Id)).ReturnsAsync(true);
            var service = new TutorSubjectService(
                tutorSubjectsRepository.Object,
                tutorsRepository.Object,
                subjectsRepository.Object,
                new Mock<IUsersRepository>().Object);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.AddSubjectToTutor(userId, subject.Id, 500m));
        }
    }
}
