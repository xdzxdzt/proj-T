using Moq;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class SolutionsServiceTests
    {
        [Test]
        public async Task SubmitSolutionWithValidDataAddsSolution()
        {
            var solutionsRepository = new Mock<ISolutionsRepository>();
            var tutorTasksRepository = new Mock<ITutorTasksRepository>();
            var studentsRepository = new Mock<IStudentsRepository>();
            var lessonPackagesRepository = new Mock<ILessonPackagesRepository>();

            var userId = Guid.NewGuid();
            var student = TestData.Student(userId: userId);
            var lessonPackage = TestData.LessonPackage(studentId: student.Id);
            var task = TestData.TutorTask(lessonPackageId: lessonPackage.Id);

            studentsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(student);
            tutorTasksRepository.Setup(x => x.GetById(task.Id)).ReturnsAsync(task);
            lessonPackagesRepository.Setup(x => x.GetById(lessonPackage.Id)).ReturnsAsync(lessonPackage);
            solutionsRepository.Setup(x => x.ExistsByTutorTaskIdAndStudentId(task.Id, student.Id)).ReturnsAsync(false);
            var service = CreateService(
                solutionsRepository: solutionsRepository,
                tutorTasksRepository: tutorTasksRepository,
                studentsRepository: studentsRepository,
                lessonPackagesRepository: lessonPackagesRepository);

            await service.SubmitSolution(userId, task.Id, "Ответ");

            solutionsRepository.Verify(x => x.Add(It.Is<Solution>(s =>
                s.TutorTaskId == task.Id &&
                s.StudentId == student.Id &&
                s.AnswerText == "Ответ")), Times.Once);
        }

        [Test]
        public void SubmitSolutionWhenStudentNotFoundThrowsInvalidOperationException()
        {
            var studentsRepository = new Mock<IStudentsRepository>();

            studentsRepository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).ReturnsAsync((Student?)null);
            var service = CreateService(studentsRepository: studentsRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.SubmitSolution(Guid.NewGuid(), Guid.NewGuid(), "Ответ"));
        }

        [Test]
        public async Task GetHistoryForStudentWithValidDataReturnsHistory()
        {
            var solutionsRepository = new Mock<ISolutionsRepository>();
            var tutorTasksRepository = new Mock<ITutorTasksRepository>();
            var studentsRepository = new Mock<IStudentsRepository>();
            var tutorFeedbacksRepository = new Mock<ITutorFeedbacksRepository>();

            var userId = Guid.NewGuid();
            var student = TestData.Student(userId: userId);
            var task = TestData.TutorTask();
            var solution = TestData.Solution(tutorTaskId: task.Id, studentId: student.Id);
            var feedback = TestData.TutorFeedback(solutionId: solution.Id);

            studentsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(student);
            solutionsRepository.Setup(x => x.GetAllByStudentId(student.Id)).ReturnsAsync(new List<Solution> { solution });
            tutorTasksRepository.Setup(x => x.GetById(task.Id)).ReturnsAsync(task);
            tutorFeedbacksRepository.Setup(x => x.GetBySolutionId(solution.Id)).ReturnsAsync(feedback);
            var service = CreateService(
                solutionsRepository: solutionsRepository,
                tutorTasksRepository: tutorTasksRepository,
                studentsRepository: studentsRepository,
                tutorFeedbacksRepository: tutorFeedbacksRepository);

            var result = await service.GetHistoryForStudent(userId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Has.Count.EqualTo(1));
                Assert.That(result[0].Solution.Id, Is.EqualTo(solution.Id));
                Assert.That(result[0].TutorTask.Id, Is.EqualTo(task.Id));
                Assert.That(result[0].TutorFeedback?.Id, Is.EqualTo(feedback.Id));
            });
        }

        private static SolutionsService CreateService(
            Mock<ISolutionsRepository>? solutionsRepository = null,
            Mock<ITutorTasksRepository>? tutorTasksRepository = null,
            Mock<IStudentsRepository>? studentsRepository = null,
            Mock<ITutorsRepository>? tutorsRepository = null,
            Mock<ILessonPackagesRepository>? lessonPackagesRepository = null,
            Mock<ITutorFeedbacksRepository>? tutorFeedbacksRepository = null)
        {
            return new SolutionsService(
                (solutionsRepository ?? new Mock<ISolutionsRepository>()).Object,
                (tutorTasksRepository ?? new Mock<ITutorTasksRepository>()).Object,
                (studentsRepository ?? new Mock<IStudentsRepository>()).Object,
                (tutorsRepository ?? new Mock<ITutorsRepository>()).Object,
                (lessonPackagesRepository ?? new Mock<ILessonPackagesRepository>()).Object,
                (tutorFeedbacksRepository ?? new Mock<ITutorFeedbacksRepository>()).Object);
        }
    }
}
