using Teachly.Core.Enums;

namespace Teachly.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public StudentEntity? Student { get; set; }
        public TutorEntity? Tutor { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public UserRole Role { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
