using CSharpFunctionalExtensions;
using Teachly.Core.Enums;

namespace Teachly.Core.Models
{
    public class User
    {
        public const int MAX_FIRSTNAME_LENGTH = 100;
        public const int MAX_LASTNAME_LENGTH = 100;
        public const int MAX_EMAIL_LENGTH = 320;
        public const int MAX_PASSWORD_HASH_LENGTH = 255;
        public const int MAX_USERNAME_LENGTH = 100;
        public const int MAX_AVATAR_URL_LENGTH = 500;

        private User(Guid id, string userName, string firstName, string lastName, int age, string email, string passwordHash, UserRole role, DateTime registeredAt)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            RegisteredAt = registeredAt;
        }

        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Age { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string? AvatarUrl { get; private set; }
        public UserRole Role { get; private set; }
        public DateTime RegisteredAt { get; private set; }

        public static Result<User> Create(Guid id, string userName, string firstName, string lastName, int age, string email, string passwordHash, UserRole role, DateTime? registeredAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<User>($"'{nameof(id)}' не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(userName) || userName.Length > MAX_USERNAME_LENGTH)
            {
                return Result.Failure<User>($"'{nameof(userName)}' не может быть пустым или длиннее {MAX_USERNAME_LENGTH} символов");
            }
            if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > MAX_FIRSTNAME_LENGTH)
            {
                return Result.Failure<User>($"'{nameof(firstName)}' не может быть пустым или длиннее {MAX_FIRSTNAME_LENGTH} символов");
            }
            if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > MAX_LASTNAME_LENGTH)
            {
                return Result.Failure<User>($"'{nameof(lastName)}' не может быть пустым или длиннее {MAX_LASTNAME_LENGTH} символов");
            }
            if (age <= 0)
            {
                return Result.Failure<User>($"'{nameof(age)}' должен быть больше 0");
            }
            if (string.IsNullOrWhiteSpace(email) || email.Length > MAX_EMAIL_LENGTH)
            {
                return Result.Failure<User>($"'{nameof(email)}' не может быть пустым или длиннее {MAX_EMAIL_LENGTH} символов");
            }
            if (string.IsNullOrWhiteSpace(passwordHash) || passwordHash.Length > MAX_PASSWORD_HASH_LENGTH)
            {
                return Result.Failure<User>($"'{nameof(passwordHash)}' не может быть пустым или длиннее {MAX_PASSWORD_HASH_LENGTH} символов");
            }

            var actualRegisteredAt = registeredAt ?? DateTime.UtcNow;

            var user = new User(id, userName, firstName, lastName, age, email, passwordHash, role, actualRegisteredAt);

            return Result.Success(user);
        }

        public Result SetAvatarUrl(string avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(avatarUrl) || avatarUrl.Length > MAX_AVATAR_URL_LENGTH)
            {
                return Result.Failure($"'{nameof(avatarUrl)}' не может быть пустым или длиннее {MAX_AVATAR_URL_LENGTH} символов");
            }

            AvatarUrl = avatarUrl;

            return Result.Success();
        }

        public Result RemoveAvatar()
        {
            AvatarUrl = null;
            return Result.Success();
        }
    }
}
