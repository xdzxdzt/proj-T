using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class TutorsRepository : ITutorsRepository
    {
        private readonly TeachlyDbContext _context;

        public TutorsRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(Tutor tutor)
        {
            var tutorEntity = new TutorEntity() 
            { 
                Id = tutor.Id,
                UserId = tutor.UserId,
                Description = tutor.Description,
                AverageRating = tutor.AverageRating,
                RatingCount = tutor.RatingCount
            };

            await _context.Tutors.AddAsync(tutorEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Tutor?> GetById(Guid id)
        {
            var tutorEntity = await _context.Tutors
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if(tutorEntity is null)
            {
                return null;
            }

            return MapToDomain(tutorEntity);
        }

        public async Task<Tutor?> GetByUserId(Guid userId)
        {
            var tutorEntity = await _context.Tutors
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (tutorEntity is null)
            {
                return null;
            }

            return MapToDomain(tutorEntity);
        }

        public async Task<bool> ExistsByUserId(Guid userId)
        {
            return await _context.Tutors
                .AsNoTracking()
                .AnyAsync(t => t.UserId == userId);
        }

        private static Tutor MapToDomain(TutorEntity tutorEntity)
        {
            var result = Tutor.Create(
                tutorEntity.Id,
                tutorEntity.UserId,
                tutorEntity.Description,
                tutorEntity.AverageRating,
                tutorEntity.RatingCount);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
