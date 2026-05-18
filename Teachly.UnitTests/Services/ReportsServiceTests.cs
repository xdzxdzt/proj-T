using Moq;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class ReportsServiceTests
    {
        [Test]
        public async Task GetStudentProgressReportForStudentWithSolutionsReturnsReport()
        {
            var studentsRepository = new Mock<IStudentsRepository>();
            var solutionsRepository = new Mock<ISolutionsRepository>();
            var tutorFeedbacksRepository = new Mock<ITutorFeedbacksRepository>();

            var userId = Guid.NewGuid();
            var student = TestData.Student(userId: userId);
            var firstSolution = TestData.Solution(studentId: student.Id);
            var secondSolution = TestData.Solution(studentId: student.Id);
            var firstFeedback = TestData.TutorFeedback(solutionId: firstSolution.Id, grade: 4);
            var secondFeedback = TestData.TutorFeedback(solutionId: secondSolution.Id, grade: 5);

            studentsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(student);
            studentsRepository.Setup(x => x.GetById(student.Id)).ReturnsAsync(student);
            solutionsRepository.Setup(x => x.GetAllByStudentId(student.Id)).ReturnsAsync(new List<Solution> { firstSolution, secondSolution });
            tutorFeedbacksRepository.Setup(x => x.GetBySolutionId(firstSolution.Id)).ReturnsAsync(firstFeedback);
            tutorFeedbacksRepository.Setup(x => x.GetBySolutionId(secondSolution.Id)).ReturnsAsync(secondFeedback);
            var service = CreateService(
                studentsRepository: studentsRepository,
                solutionsRepository: solutionsRepository,
                tutorFeedbacksRepository: tutorFeedbacksRepository);

            var result = await service.GetStudentProgressReportForStudent(userId);

            Assert.Multiple(() =>
            {
                Assert.That(result.StudentId, Is.EqualTo(student.Id));
                Assert.That(result.SubmittedSolutions, Is.EqualTo(2));
                Assert.That(result.CheckedSolutions, Is.EqualTo(2));
                Assert.That(result.AverageGrade, Is.EqualTo(4.5));
            });
        }

        [Test]
        public void GetStudentProgressReportForStudentWhenStudentNotFoundThrowsInvalidOperationException()
        {
            var studentsRepository = new Mock<IStudentsRepository>();

            studentsRepository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).ReturnsAsync((Student?)null);
            var service = CreateService(studentsRepository: studentsRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.GetStudentProgressReportForStudent(Guid.NewGuid()));
        }

        private static ReportsService CreateService(
            Mock<IStudentsRepository>? studentsRepository = null,
            Mock<ITutorsRepository>? tutorsRepository = null,
            Mock<ILessonPackagesRepository>? lessonPackagesRepository = null,
            Mock<ISolutionsRepository>? solutionsRepository = null,
            Mock<ITutorFeedbacksRepository>? tutorFeedbacksRepository = null)
        {
            return new ReportsService(
                (studentsRepository ?? new Mock<IStudentsRepository>()).Object,
                (tutorsRepository ?? new Mock<ITutorsRepository>()).Object,
                (lessonPackagesRepository ?? new Mock<ILessonPackagesRepository>()).Object,
                (solutionsRepository ?? new Mock<ISolutionsRepository>()).Object,
                (tutorFeedbacksRepository ?? new Mock<ITutorFeedbacksRepository>()).Object);
        }
    }
}
