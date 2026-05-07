using Teachly.Application.Interfaces.Auth;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Enums;
using Teachly.Core.Models;

namespace Teachly.Application.Services
{
    public class UsersService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtProvider _jwtProvider;

        public UsersService(IPasswordHasher passwordHasher, IUsersRepository usersRepository, IJwtProvider jwtProvider)
        {
            _passwordHasher = passwordHasher;
            _usersRepository = usersRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task Register(string userName, string firstName, string lastName, int age, string email, string password, UserRole role)
        {
            if (await _usersRepository.ExistsByEmail(email))
            {
                throw new InvalidOperationException("Пользователь с таким Email уже существует");
            }

            var hashedPassword = _passwordHasher.Generate(password);

            var user = User.Create(Guid.NewGuid(), userName, firstName, lastName, age, email, hashedPassword, role);

            if (user.IsFailure)
            {
                throw new InvalidOperationException(user.Error);
            }

            await _usersRepository.Add(user.Value);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _usersRepository.GetByEmail(email);

            if (user is null)
            {
                throw new Exception("Пользователя с таким Email не найдено");
            }

            var result = _passwordHasher.Verify(password, user.PasswordHash);

            if(result == false)
            {
                throw new Exception("Не правильно введен пароль");
            }

            var token = _jwtProvider.GenerateToken(user);

            return token;
        }
    }
}
