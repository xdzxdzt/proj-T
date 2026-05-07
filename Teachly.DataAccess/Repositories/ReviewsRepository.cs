using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly TeachlyDbContext _context;

        public ReviewsRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(Review review)
        {
            var reviewEntity = new ReviewEntity()
            {
                Id = review.Id,
                StudentId = review.StudentId,
                TutorId = review.TutorId,
                ReviewText = review.ReviewText,
                Rating = review.Rating,
                CreatedAt = review.CreatedAt,
            };

            await _context.Reviews.AddAsync(reviewEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Review>> GetByTutorId(Guid tutorId)
        {
            var reviewEntities = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.TutorId == tutorId)
                .ToListAsync();

            var reviews = reviewEntities
                .Select(MapToDomain)
                .ToList();

            return reviews;
        }

        public async Task<bool> Exists(Guid studentId, Guid tutorId)
        {
            return await _context.Reviews
                .AsNoTracking()
                .AnyAsync(r => r.TutorId == tutorId && r.StudentId == studentId);
        }

        private static Review MapToDomain(ReviewEntity reviewEntity)
        {
            var result = Review.Create(
                reviewEntity.Id,
                reviewEntity.StudentId,
                reviewEntity.TutorId,
                reviewEntity.ReviewText,
                reviewEntity.Rating,
                reviewEntity.CreatedAt);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
