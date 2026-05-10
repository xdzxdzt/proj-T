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
    }
}
