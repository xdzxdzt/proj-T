using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class TutorFeedbacksRepository : ITutorFeedbacksRepository
    {
        private readonly TeachlyDbContext _context;

        public TutorFeedbacksRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(TutorFeedback tutorFeedback)
        {
            var tutorFeedbackEntity = new TutorFeedbackEntity()
            {
                Id = tutorFeedback.Id,
                SolutionId = tutorFeedback.SolutionId,
                TutorComment = tutorFeedback.TutorComment,
                Grade = tutorFeedback.Grade,
                GivenAt = tutorFeedback.GivenAt,
            };

            await _context.TutorFeedbacks.AddAsync(tutorFeedbackEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<TutorFeedback?> GetBySolutionId(Guid solutionId)
        {
            var tutorFeedbackEntity = await _context.TutorFeedbacks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.SolutionId == solutionId);

            if(tutorFeedbackEntity is null)
            {
                return null;
            }

            var result = TutorFeedback.Create(
                tutorFeedbackEntity.Id,
                tutorFeedbackEntity.SolutionId,
                tutorFeedbackEntity.TutorComment,
                tutorFeedbackEntity.Grade,
                tutorFeedbackEntity.GivenAt
                );

            if(result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }

        public async Task<bool> ExistsBySolutionId(Guid solutionId)
        {
            return await _context.TutorFeedbacks
                .AsNoTracking()
                .Where(t => t.SolutionId == solutionId)
                .AnyAsync();
        }
    }
}
