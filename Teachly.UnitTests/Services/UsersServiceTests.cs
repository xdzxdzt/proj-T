using Moq;
using Teachly.Application.Interfaces.Auth;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Services;
using Teachly.Core.Enums;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Services
{
    public class UsersServiceTests
    {
        [Test]
        public async Task RegisterStudentWithValidDataAddsUserAndStudent()
        {
            var passwordHasher = new Mock<IPasswordHasher>();
            var usersRepository = new Mock<IUsersRepository>();
            var studentsRepository = new Mock<IStudentsRepository>();

            var email = "student@mail.ru";
            var password = "password";
            var passwordHash = "passwordHash";

            usersRepository.Setup(x => x.ExistsByEmail(email)).ReturnsAsync(false);
            passwordHasher.Setup(x => x.Generate(password)).Returns(passwordHash);
            var service = CreateService(passwordHasher: passwordHasher, usersRepository: usersRepository, studentsRepository: studentsRepository);

            await service.RegisterStudent(
                "User123",
                "Ivan",
                "Ivanov",
                16,
                email,
                password,
                null,
                9,
                "+79990000000");

            usersRepository.Verify(x => x.Add(It.Is<User>(u => u.Email == email && u.PasswordHash == passwordHash && u.Role == UserRole.Student)), Times.Once);
            studentsRepository.Verify(x => x.Add(It.Is<Student>(s => s.EducationLevel == 9)), Times.Once);
        }

        [Test]
        public void RegisterStudentWhenEmailExistsThrowsInvalidOperationException()
        {
            var usersRepository = new Mock<IUsersRepository>();

            usersRepository.Setup(x => x.ExistsByEmail("student@mail.ru")).ReturnsAsync(true);
            var service = CreateService(usersRepository: usersRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterStudent(
                "User123",
                "Ivan",
                "Ivanov",
                16,
                "student@mail.ru",
                "password",
                null,
                9,
                null));
        }

        [Test]
        public async Task LoginWithValidDataReturnsToken()
        {
            var passwordHasher = new Mock<IPasswordHasher>();
            var usersRepository = new Mock<IUsersRepository>();
            var jwtProvider = new Mock<IJwtProvider>();

            var user = TestData.User();
            var password = "password";
            var token = "jwt-token";

            usersRepository.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);
            passwordHasher.Setup(x => x.Verify(password, user.PasswordHash)).Returns(true);
            jwtProvider.Setup(x => x.GenerateToken(user)).Returns(token);
            var service = CreateService(passwordHasher: passwordHasher, usersRepository: usersRepository, jwtProvider: jwtProvider);

            var result = await service.Login(user.Email, password);

            Assert.That(result, Is.EqualTo(token));
        }

        [Test]
        public void LoginWithInvalidPasswordThrowsInvalidOperationException()
        {
            var passwordHasher = new Mock<IPasswordHasher>();
            var usersRepository = new Mock<IUsersRepository>();

            var user = TestData.User();

            usersRepository.Setup(x => x.GetByEmail(user.Email)).ReturnsAsync(user);
            passwordHasher.Setup(x => x.Verify("bad-password", user.PasswordHash)).Returns(false);
            var service = CreateService(passwordHasher: passwordHasher, usersRepository: usersRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.Login(user.Email, "bad-password"));
        }

        [Test]
        public async Task SetAvatarWithValidDataUpdatesUser()
        {
            var usersRepository = new Mock<IUsersRepository>();

            var user = TestData.User();
            var avatarUrl = "/uploads/avatars/avatar.png";

            usersRepository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
            var service = CreateService(usersRepository: usersRepository);

            await service.SetAvatar(user.Id, avatarUrl);

            usersRepository.Verify(x => x.Update(It.Is<User>(u => u.Id == user.Id && u.AvatarUrl == avatarUrl)), Times.Once);
        }

        [Test]
        public void SetAvatarWhenUserNotFoundThrowsInvalidOperationException()
        {
            var usersRepository = new Mock<IUsersRepository>();

            usersRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((User?)null);
            var service = CreateService(usersRepository: usersRepository);

            Assert.ThrowsAsync<InvalidOperationException>(() => service.SetAvatar(Guid.NewGuid(), "/uploads/avatars/avatar.png"));
        }

        private static UsersService CreateService(
            Mock<IPasswordHasher>? passwordHasher = null,
            Mock<IUsersRepository>? usersRepository = null,
            Mock<IStudentsRepository>? studentsRepository = null,
            Mock<ITutorsRepository>? tutorsRepository = null,
            Mock<IInstitutionsRepository>? institutionsRepository = null,
            Mock<IJwtProvider>? jwtProvider = null)
        {
            return new UsersService(
                (passwordHasher ?? new Mock<IPasswordHasher>()).Object,
                (usersRepository ?? new Mock<IUsersRepository>()).Object,
                (studentsRepository ?? new Mock<IStudentsRepository>()).Object,
                (tutorsRepository ?? new Mock<ITutorsRepository>()).Object,
                (institutionsRepository ?? new Mock<IInstitutionsRepository>()).Object,
                (jwtProvider ?? new Mock<IJwtProvider>()).Object);
        }
    }
}
