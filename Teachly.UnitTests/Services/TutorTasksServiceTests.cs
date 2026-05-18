using Moq;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class TutorTasksServiceTests
    {
        [Test]
        public async Task CreateTaskWithValidDataAddsTutorTask()
        {
            var tutorTasksRepository = new Mock<ITutorTasksRepository>();
            var lessonPackagesRepository = new Mock<ILessonPackagesRepository>();
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();

            var userId = Guid.NewGuid();
            var tutor = TestData.Tutor(userId: userId);
            var tutorSubject = TestData.TutorSubject(tutorId: tutor.Id);
            var lessonPackage = TestData.LessonPackage(tutorSubjectId: tutorSubject.Id);

            tutorsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(tutor);
            lessonPackagesRepository.Setup(x => x.GetById(lessonPackage.Id)).ReturnsAsync(lessonPackage);
            tutorSubjectsRepository.Setup(x => x.GetById(tutorSubject.Id)).ReturnsAsync(tutorSubject);
            var service = CreateService(
                tutorTasksRepository: tutorTasksRepository,
                lessonPackagesRepository: lessonPackagesRepository,
                tutorSubjectsRepository: tutorSubjectsRepository,
                tutorsRepository: tutorsRepository);

            await service.CreateTask(userId, lessonPackage.Id, "Задание", "Описание задания");

            tutorTasksRepository.Verify(x => x.Add(It.Is<TutorTask>(t =>
                t.LessonPackageId == lessonPackage.Id &&
                t.Title == "Задание" &&
                t.Description == "Описание задания")), Times.Once);
        }

        [Test]
        public void CreateTaskWhenTutorNotFoundThrowsInvalidOperationException()
        {
            var tutorsRepository = new Mock<ITutorsRepository>();

            tutorsRepository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).ReturnsAsync((Tutor?)null);
            var service = CreateService(tutorsRepository: tutorsRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateTask(Guid.NewGuid(), Guid.NewGuid(), "Задание", "Описание задания"));
        }

        [Test]
        public async Task CloseTaskWithValidDataUpdatesTutorTask()
        {
            var tutorTasksRepository = new Mock<ITutorTasksRepository>();
            var lessonPackagesRepository = new Mock<ILessonPackagesRepository>();
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();

            var userId = Guid.NewGuid();
            var tutor = TestData.Tutor(userId: userId);
            var tutorSubject = TestData.TutorSubject(tutorId: tutor.Id);
            var lessonPackage = TestData.LessonPackage(tutorSubjectId: tutorSubject.Id);
            var task = TestData.TutorTask(lessonPackageId: lessonPackage.Id);

            tutorsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(tutor);
            tutorTasksRepository.Setup(x => x.GetById(task.Id)).ReturnsAsync(task);
            lessonPackagesRepository.Setup(x => x.GetById(lessonPackage.Id)).ReturnsAsync(lessonPackage);
            tutorSubjectsRepository.Setup(x => x.GetById(tutorSubject.Id)).ReturnsAsync(tutorSubject);
            var service = CreateService(
                tutorTasksRepository: tutorTasksRepository,
                lessonPackagesRepository: lessonPackagesRepository,
                tutorSubjectsRepository: tutorSubjectsRepository,
                tutorsRepository: tutorsRepository);

            await service.CloseTask(userId, task.Id);

            tutorTasksRepository.Verify(x => x.Update(It.Is<TutorTask>(t => t.Id == task.Id && t.ClosedAt != null)), Times.Once);
        }

        private static TutorTasksService CreateService(
            Mock<ITutorTasksRepository>? tutorTasksRepository = null,
            Mock<ILessonPackagesRepository>? lessonPackagesRepository = null,
            Mock<ITutorSubjectsRepository>? tutorSubjectsRepository = null,
            Mock<ITutorsRepository>? tutorsRepository = null,
            Mock<IStudentsRepository>? studentsRepository = null)
        {
            return new TutorTasksService(
                (tutorTasksRepository ?? new Mock<ITutorTasksRepository>()).Object,
                (lessonPackagesRepository ?? new Mock<ILessonPackagesRepository>()).Object,
                (tutorSubjectsRepository ?? new Mock<ITutorSubjectsRepository>()).Object,
                (tutorsRepository ?? new Mock<ITutorsRepository>()).Object,
                (studentsRepository ?? new Mock<IStudentsRepository>()).Object);
        }
    }
}
