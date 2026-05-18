using Moq;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class ReviewsServiceTests
    {
        [Test]
        public async Task CreateReviewWithValidDataAddsReviewAndUpdatesTutor()
        {
            var reviewsRepository = new Mock<IReviewsRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();
            var studentsRepository = new Mock<IStudentsRepository>();

            var userId = Guid.NewGuid();
            var student = TestData.Student(userId: userId);
            var tutor = TestData.Tutor();

            studentsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(student);
            tutorsRepository.Setup(x => x.GetById(tutor.Id)).ReturnsAsync(tutor);
            reviewsRepository.Setup(x => x.Exists(student.Id, tutor.Id)).ReturnsAsync(false);
            var service = new ReviewsService(reviewsRepository.Object, tutorsRepository.Object, studentsRepository.Object);

            await service.CreateReview(userId, tutor.Id, "Отзыв", 5);

            reviewsRepository.Verify(x => x.Add(It.Is<Review>(r =>
                r.StudentId == student.Id &&
                r.TutorId == tutor.Id &&
                r.Rating == 5)), Times.Once);
            tutorsRepository.Verify(x => x.Update(It.Is<Tutor>(t =>
                t.Id == tutor.Id &&
                t.RatingCount == 1 &&
                t.AverageRating == 5)), Times.Once);
        }

        [Test]
        public void CreateReviewWhenReviewAlreadyExistsThrowsInvalidOperationException()
        {
            var reviewsRepository = new Mock<IReviewsRepository>();
            var tutorsRepository = new Mock<ITutorsRepository>();
            var studentsRepository = new Mock<IStudentsRepository>();

            var userId = Guid.NewGuid();
            var student = TestData.Student(userId: userId);
            var tutor = TestData.Tutor();

            studentsRepository.Setup(x => x.GetByUserId(userId)).ReturnsAsync(student);
            tutorsRepository.Setup(x => x.GetById(tutor.Id)).ReturnsAsync(tutor);
            reviewsRepository.Setup(x => x.Exists(student.Id, tutor.Id)).ReturnsAsync(true);
            var service = new ReviewsService(reviewsRepository.Object, tutorsRepository.Object, studentsRepository.Object);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateReview(userId, tutor.Id, "Отзыв", 5));
        }
    }
}
