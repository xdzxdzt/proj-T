using Moq;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class LessonPackagesServiceTests
    {
        [Test]
        public async Task BuyPackageWithValidDataAddsLessonPackage()
        {
            var lessonPackagesRepository = new Mock<ILessonPackagesRepository>();
            var studentsRepository = new Mock<IStudentsRepository>();
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();

            var userId = Guid.NewGuid();
            var student = TestData.Student(userId: userId);
            var tutorSubject = TestData.TutorSubject();

            studentsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(student);
            tutorSubjectsRepository.Setup(x => x.GetById(tutorSubject.Id)).ReturnsAsync(tutorSubject);
            var service = CreateService(
                lessonPackagesRepository: lessonPackagesRepository,
                studentsRepository: studentsRepository,
                tutorSubjectsRepository: tutorSubjectsRepository);

            await service.BuyPackage(userId, tutorSubject.Id, 4);

            lessonPackagesRepository.Verify(x => x.Add(It.Is<LessonPackage>(p =>
                p.StudentId == student.Id &&
                p.TutorSubjectId == tutorSubject.Id &&
                p.TotalLessons == 4)), Times.Once);
        }

        [Test]
        public void BuyPackageWhenStudentNotFoundThrowsInvalidOperationException()
        {
            var studentsRepository = new Mock<IStudentsRepository>();

            studentsRepository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).ReturnsAsync((Student?)null);
            var service = CreateService(studentsRepository: studentsRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.BuyPackage(Guid.NewGuid(), Guid.NewGuid(), 4));
        }

        [Test]
        public async Task UseLessonWithValidDataUpdatesLessonPackage()
        {
            var lessonPackagesRepository = new Mock<ILessonPackagesRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();

            var userId = Guid.NewGuid();
            var tutor = TestData.Tutor(userId: userId);
            var tutorSubject = TestData.TutorSubject(tutorId: tutor.Id);
            var lessonPackage = TestData.LessonPackage(tutorSubjectId: tutorSubject.Id);

            tutorsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(tutor);
            lessonPackagesRepository.Setup(x => x.GetById(lessonPackage.Id)).ReturnsAsync(lessonPackage);
            tutorSubjectsRepository.Setup(x => x.GetById(tutorSubject.Id)).ReturnsAsync(tutorSubject);
            var service = CreateService(
                lessonPackagesRepository: lessonPackagesRepository,
                tutorsRepository: tutorsRepository,
                tutorSubjectsRepository: tutorSubjectsRepository);

            await service.UseLesson(userId, lessonPackage.Id);

            lessonPackagesRepository.Verify(x => x.Update(It.Is<LessonPackage>(p => p.Id == lessonPackage.Id && p.RemainingLessons == 3)), Times.Once);
        }

        [Test]
        public void UseLessonWithForeignPackageThrowsInvalidOperationException()
        {
            var lessonPackagesRepository = new Mock<ILessonPackagesRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();

            var userId = Guid.NewGuid();
            var tutor = TestData.Tutor(userId: userId);
            var tutorSubject = TestData.TutorSubject(tutorId: Guid.NewGuid());
            var lessonPackage = TestData.LessonPackage(tutorSubjectId: tutorSubject.Id);

            tutorsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(tutor);
            lessonPackagesRepository.Setup(x => x.GetById(lessonPackage.Id)).ReturnsAsync(lessonPackage);
            tutorSubjectsRepository.Setup(x => x.GetById(tutorSubject.Id)).ReturnsAsync(tutorSubject);
            var service = CreateService(
                lessonPackagesRepository: lessonPackagesRepository,
                tutorsRepository: tutorsRepository,
                tutorSubjectsRepository: tutorSubjectsRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.UseLesson(userId, lessonPackage.Id));
        }

        private static LessonPackagesService CreateService(
            Mock<ILessonPackagesRepository>? lessonPackagesRepository = null,
            Mock<IStudentsRepository>? studentsRepository = null,
            Mock<ITutorsRepository>? tutorsRepository = null,
            Mock<ITutorSubjectsRepository>? tutorSubjectsRepository = null,
            Mock<ISubjectsRepository>? subjectsRepository = null,
            Mock<IUsersRepository>? usersRepository = null)
        {
            return new LessonPackagesService(
                (lessonPackagesRepository ?? new Mock<ILessonPackagesRepository>()).Object,
                (studentsRepository ?? new Mock<IStudentsRepository>()).Object,
                (tutorsRepository ?? new Mock<ITutorsRepository>()).Object,
                (tutorSubjectsRepository ?? new Mock<ITutorSubjectsRepository>()).Object,
                (subjectsRepository ?? new Mock<ISubjectsRepository>()).Object,
                (usersRepository ?? new Mock<IUsersRepository>()).Object);
        }
    }
}
