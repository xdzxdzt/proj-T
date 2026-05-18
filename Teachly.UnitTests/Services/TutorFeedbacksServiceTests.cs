using Moq;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class TutorFeedbacksServiceTests
    {
        [Test]
        public async Task GiveFeedbackWithValidDataAddsFeedback()
        {
            var tutorFeedbacksRepository = new Mock<ITutorFeedbacksRepository>();
            var solutionsRepository = new Mock<ISolutionsRepository>();
            var tutorTasksRepository = new Mock<ITutorTasksRepository>();
            var lessonPackagesRepository = new Mock<ILessonPackagesRepository>();
            var tutorSubjectsRepository = new Mock<ITutorSubjectsRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();

            var userId = Guid.NewGuid();
            var tutor = TestData.Tutor(userId: userId);
            var tutorSubject = TestData.TutorSubject(tutorId: tutor.Id);
            var lessonPackage = TestData.LessonPackage(tutorSubjectId: tutorSubject.Id);
            var task = TestData.TutorTask(lessonPackageId: lessonPackage.Id);
            var solution = TestData.Solution(tutorTaskId: task.Id);

            tutorsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(tutor);
            solutionsRepository.Setup(x => x.GetById(solution.Id)).ReturnsAsync(solution);
            tutorTasksRepository.Setup(x => x.GetById(task.Id)).ReturnsAsync(task);
            lessonPackagesRepository.Setup(x => x.GetById(lessonPackage.Id)).ReturnsAsync(lessonPackage);
            tutorSubjectsRepository.Setup(x => x.GetById(tutorSubject.Id)).ReturnsAsync(tutorSubject);
            tutorFeedbacksRepository.Setup(x => x.ExistsBySolutionId(solution.Id)).ReturnsAsync(false);
            var service = new TutorFeedbacksService(
                tutorFeedbacksRepository.Object,
                solutionsRepository.Object,
                tutorTasksRepository.Object,
                lessonPackagesRepository.Object,
                tutorSubjectsRepository.Object,
                tutorsRepository.Object);

            await service.GiveFeedback(userId, solution.Id, 5, "Комментарий");

            tutorFeedbacksRepository.Verify(x => x.Add(It.Is<TutorFeedback>(f =>
                f.SolutionId == solution.Id &&
                f.Grade == 5 &&
                f.TutorComment == "Комментарий")), Times.Once);
        }

        [Test]
        public void GiveFeedbackWhenTutorNotFoundThrowsInvalidOperationException()
        {
            var tutorsRepository = new Mock<ITutorsRepository>();

            tutorsRepository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).ReturnsAsync((Tutor?)null);
            var service = new TutorFeedbacksService(
                new Mock<ITutorFeedbacksRepository>().Object,
                new Mock<ISolutionsRepository>().Object,
                new Mock<ITutorTasksRepository>().Object,
                new Mock<ILessonPackagesRepository>().Object,
                new Mock<ITutorSubjectsRepository>().Object,
                tutorsRepository.Object);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.GiveFeedback(Guid.NewGuid(), Guid.NewGuid(), 5, "Комментарий"));
        }
    }
}
