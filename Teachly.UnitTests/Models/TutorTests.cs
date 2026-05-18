using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class TutorTests
    {
        private static IEnumerable<TestCaseData> InvalidTutorData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                "Описание репетитора",
                0m,
                0,
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                "Описание репетитора",
                0m,
                0,
                "userId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new string('a', Tutor.MAX_DESCRIPTION_LENGTH + 1),
                0m,
                0,
                "description");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Описание репетитора",
                -1m,
                0,
                "averageRating");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Описание репетитора",
                6m,
                0,
                "averageRating");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Описание репетитора",
                0m,
                -1,
                "ratingCount");
        }

        [Test]
        public void CreateTutorWithValidData()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var description = "Описание репетитора";

            var result = Tutor.Create(id, userId, description);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.UserId, Is.EqualTo(userId));
                Assert.That(result.Value.Description, Is.EqualTo(description));
                Assert.That(result.Value.AverageRating, Is.EqualTo(0));
                Assert.That(result.Value.RatingCount, Is.EqualTo(0));
            });
        }

        [TestCaseSource(nameof(InvalidTutorData))]
        public void CreateTutorWithInvalidData(
            Guid id,
            Guid userId,
            string? description,
            decimal averageRating,
            int ratingCount,
            string expectedError)
        {
            var result = Tutor.Create(
                id,
                userId,
                description,
                averageRating,
                ratingCount);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }

        [TestCase((short)5, (short)3, 4)]
        [TestCase((short)5, (short)4, 4.5)]
        [TestCase((short)2, (short)5, 3.5)]
        public void AddSeveralRatingsToTutor(short firstRating, short secondRating, decimal expectedAverageRating)
        {
            var tutor = Tutor.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Описание репетитора").Value;

            var firstResult = tutor.AddRating(firstRating);
            var secondResult = tutor.AddRating(secondRating);

            Assert.Multiple(() =>
            {
                Assert.That(firstResult.IsSuccess, Is.True);
                Assert.That(secondResult.IsSuccess, Is.True);
                Assert.That(tutor.RatingCount, Is.EqualTo(2));
                Assert.That(tutor.AverageRating, Is.EqualTo(expectedAverageRating));
            });
        }

        [TestCase((short)-1)]
        [TestCase((short)0)]
        [TestCase((short)6)]
        public void AddRatingToTutorWithInvalidRating(short rating)
        {
            var tutor = Tutor.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Описание репетитора").Value;

            var result = tutor.AddRating(rating);

            Assert.That(result.IsFailure, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Error, Does.Contain("newRating"));
                Assert.That(tutor.AverageRating, Is.EqualTo(0));
                Assert.That(tutor.RatingCount, Is.EqualTo(0));
            });
        }
    }
}
