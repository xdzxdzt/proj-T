using Teachly.Core.Enums;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class UserTests
    {
        private static IEnumerable<TestCaseData> InvalidUserData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                "User123",
                "Ivan",
                "Ivanov",
                16,
                "IvanIvanov@mail.ru",
                "passwordHash",
                UserRole.Student,
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                "",
                "Ivan",
                "Ivanov",
                16,
                "IvanIvanov@mail.ru",
                "passwordHash",
                UserRole.Student,
                "userName");

            yield return new TestCaseData(
                Guid.NewGuid(),
                "User123",
                "",
                "Ivanov",
                16,
                "IvanIvanov@mail.ru",
                "passwordHash",
                UserRole.Student,
                "firstName");

            yield return new TestCaseData(
                Guid.NewGuid(),
                "User123",
                "Ivan",
                "",
                16,
                "IvanIvanov@mail.ru",
                "passwordHash",
                UserRole.Student,
                "lastName");

            yield return new TestCaseData(
                Guid.NewGuid(),
                "User123",
                "Ivan",
                "Ivanov",
                0,
                "IvanIvanov@mail.ru",
                "passwordHash",
                UserRole.Student,
                "age");

            yield return new TestCaseData(
                Guid.NewGuid(),
                "User123",
                "Ivan",
                "Ivanov",
                16,
                "",
                "passwordHash",
                UserRole.Student,
                "email");

            yield return new TestCaseData(
                Guid.NewGuid(),
                "User123",
                "Ivan",
                "Ivanov",
                16,
                "IvanIvanov@mail.ru",
                "",
                UserRole.Student,
                "passwordHash");
        }

        [Test]
        public void CreateUserWithValidData()
        {
            var id = Guid.NewGuid();
            var userName = "User123";
            var firstName = "Ivan";
            var lastName = "Ivanov";
            var age = 16;
            var email = "IvanIvanov@mail.ru";
            var passwordHash = "passwordHash";
            var userRole = UserRole.Student;
            var registeredAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            var result = User.Create(
                id,
                userName,
                firstName,
                lastName,
                age,
                email,
                passwordHash,
                userRole,
                registeredAt);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.UserName, Is.EqualTo(userName));
                Assert.That(result.Value.FirstName, Is.EqualTo(firstName));
                Assert.That(result.Value.LastName, Is.EqualTo(lastName));
                Assert.That(result.Value.Age, Is.EqualTo(age));
                Assert.That(result.Value.Email, Is.EqualTo(email));
                Assert.That(result.Value.PasswordHash, Is.EqualTo(passwordHash));
                Assert.That(result.Value.Role, Is.EqualTo(userRole));
                Assert.That(result.Value.RegisteredAt, Is.EqualTo(registeredAt));
                Assert.That(result.Value.AvatarUrl, Is.Null);
            });
        }

        [TestCaseSource(nameof(InvalidUserData))]
        public void CreateUserWithInvalidData(
            Guid id,
            string userName,
            string firstName,
            string lastName,
            int age,
            string email,
            string passwordHash,
            UserRole userRole,
            string expectedError)
        {
            var registeredAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            var result = User.Create(
                id,
                userName,
                firstName,
                lastName,
                age,
                email,
                passwordHash,
                userRole,
                registeredAt);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }

        [Test]
        public void SetAvatarUrlWithValidData()
        {
            var user = CreateValidUser();
            var avatarUrl = "/uploads/avatars/avatar.png";

            var result = user.SetAvatarUrl(avatarUrl);

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.AvatarUrl, Is.EqualTo(avatarUrl));
        }

        [TestCase("")]
        [TestCase("   ")]
        public void SetAvatarUrlWithInvalidData(string avatarUrl)
        {
            var user = CreateValidUser();

            var result = user.SetAvatarUrl(avatarUrl);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain("avatarUrl"));
            Assert.That(user.AvatarUrl, Is.Null);
        }

        [Test]
        public void RemoveAvatarClearsAvatarUrl()
        {
            var user = CreateValidUser();
            user.SetAvatarUrl("/uploads/avatars/avatar.png");

            var result = user.RemoveAvatar();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.AvatarUrl, Is.Null);
        }

        private static User CreateValidUser()
        {
            return User.Create(
                Guid.NewGuid(),
                "User123",
                "Ivan",
                "Ivanov",
                16,
                "IvanIvanov@mail.ru",
                "passwordHash",
                UserRole.Student,
                new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc)).Value;
        }
    }
}
