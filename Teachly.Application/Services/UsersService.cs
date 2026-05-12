using Teachly.Application.Interfaces.Auth;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Application.Interfaces.Services;
using Teachly.Core.Enums;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IStudentsRepository _studentsRepository;
        private readonly ITutorsRepository _tutorsRepository;
        private readonly IInstitutionsRepository _institutionsRepository;
        private readonly IJwtProvider _jwtProvider;

        public UsersService(
            IPasswordHasher passwordHasher,
            IUsersRepository usersRepository,
            IStudentsRepository studentsRepository,
            ITutorsRepository tutorsRepository,
            IInstitutionsRepository institutionsRepository,
            IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _studentsRepository = studentsRepository;
            _tutorsRepository = tutorsRepository;
            _institutionsRepository = institutionsRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task RegisterStudent(
            string userName,
            string firstName,
            string lastName,
            int age,
            string email,
            string password,
            Guid? institutionId,
            int educationLevel,
            string? parentPhone)
        {
            if (await _usersRepository.ExistsByEmail(email))
            {
                throw new InvalidOperationException("Пользователь с таким Email уже существует");
            }

            if (institutionId is not null)
            {
                var institution = await _institutionsRepository.GetById(institutionId.Value);

                if (institution is null)
                {
                    throw new InvalidOperationException("Учебное заведение не найдено");
                }
            }

            var hashedPassword = _passwordHasher.Generate(password);

            var user = User.Create(
                Guid.NewGuid(),
                userName,
                firstName,
                lastName,
                age,
                email,
                hashedPassword,
                UserRole.Student);

            if (user.IsFailure)
            {
                throw new InvalidOperationException(user.Error);
            }

            var student = Student.Create(
                Guid.NewGuid(),
                user.Value.Id,
                institutionId,
                educationLevel,
                parentPhone);

            if (student.IsFailure)
            {
                throw new InvalidOperationException(student.Error);
            }

            await _usersRepository.Add(user.Value);
            await _studentsRepository.Add(student.Value);
        }

        public async Task RegisterTutor(
            string userName,
            string firstName,
            string lastName,
            int age,
            string email,
            string password,
            string? description)
        {
            if (await _usersRepository.ExistsByEmail(email))
            {
                throw new InvalidOperationException("Пользователь с таким Email уже существует");
            }

            var hashedPassword = _passwordHasher.Generate(password);

            var user = User.Create(
                Guid.NewGuid(),
                userName,
                firstName,
                lastName,
                age,
                email,
                hashedPassword,
                UserRole.Tutor);

            if (user.IsFailure)
            {
                throw new InvalidOperationException(user.Error);
            }

            var tutor = Tutor.Create(
                Guid.NewGuid(),
                user.Value.Id,
                description);

            if (tutor.IsFailure)
            {
                throw new InvalidOperationException(tutor.Error);
            }

            await _usersRepository.Add(user.Value);
            await _tutorsRepository.Add(tutor.Value);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);

            if (user is null)
            {
                throw new InvalidOperationException("Пользователя с таким Email не найдено");
            }

            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if (result == false)
            {
                throw new InvalidOperationException("Неправильно введен пароль");
            }

            return _jwtProvider.GenerateToken(user);
        }

        public async Task<(Student Student, User User)> GetStudentProfile(Guid studentId)
        {
            var student = await _studentsRepository.GetById(studentId);

            if (student is null)
            {
                throw new InvalidOperationException("Обучающийся не найден");
            }

            var user = await _usersRepository.GetById(student.UserId);

            if (user is null)
            {
                throw new InvalidOperationException("Пользователь обучающегося не найден");
            }

            return (student, user);
        }

        public async Task<(Tutor Tutor, User User)> GetTutorProfile(Guid tutorId)
        {
            var tutor = await _tutorsRepository.GetById(tutorId);

            if (tutor is null)
            {
                throw new InvalidOperationException("Репетитор не найден");
            }

            var user = await _usersRepository.GetById(tutor.UserId);

            if (user is null)
            {
                throw new InvalidOperationException("Пользователь репетитора не найден");
            }

            return (tutor, user);
        }

        public async Task SetAvatar(Guid userId, string avatarUrl)
        {
            var user = await _usersRepository.GetById(userId);

            if (user is null)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }

            var result = user.SetAvatarUrl(avatarUrl);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            await _usersRepository.Update(user);
        }
    }
}
