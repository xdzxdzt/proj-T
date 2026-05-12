using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Services
{
    public interface IUsersService
    {
        Task RegisterStudent(
            string userName,
            string firstName,
            string lastName,
            int age,
            string email,
            string password,
            Guid? institutionId,
            int educationLevel,
            string? parentPhone);

        Task RegisterTutor(
            string userName,
            string firstName,
            string lastName,
            int age,
            string email,
            string password,
            string? description);

        Task<string> Login(string email, string password);

        Task<(Student Student, User User)> GetStudentProfile(Guid studentId);

        Task<(Tutor Tutor, User User)> GetTutorProfile(Guid tutorId);

        Task SetAvatar(Guid userId, string avatarUrl);
    }
}
