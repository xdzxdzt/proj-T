namespace Teachly.Application.Interfaces.Services
{
    public interface IReviewsService
    {
        Task CreateReview(Guid userId, Guid tutorId, string reviewText, short rating);
    }
}
