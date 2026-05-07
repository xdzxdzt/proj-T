using Teachly.Core.Models;

namespace Teachly.Application.Interfaces.Repositories
{
    public interface IReviewsRepository
    {
        Task Add(Review review);
        Task<List<Review>> GetByTutorId(Guid tutorId);
        Task<bool> Exists(Guid studentId, Guid tutorId);
    }
}
