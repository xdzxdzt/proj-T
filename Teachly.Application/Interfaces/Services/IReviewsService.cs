namespace Teachly.Application.Interfaces.Services
{
    public interface IReviewsService
    {
        Task CreateReview(Guid tutorId, Guid studentId, string reviewText, short rating);
    }
}
