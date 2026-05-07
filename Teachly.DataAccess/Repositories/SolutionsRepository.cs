using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class SolutionsRepository : ISolutionsRepository
    {
        private readonly TeachlyDbContext _context;

        public SolutionsRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(Solution solution)
        {
            var solutionEntity = new SolutionEntity()
            {
                Id = solution.Id,
                TutorTaskId = solution.TutorTaskId,
                StudentId = solution.StudentId,
                AnswerText = solution.AnswerText,
                SubmittedAt = solution.SubmittedAt
            };

            await _context.Solutions.AddAsync(solutionEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Solution?> GetById(Guid id)
        {
            var solutionEntity = await _context.Solutions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if(solutionEntity is null)
            {
                return null;
            }

            return MapToDomain(solutionEntity);
        }

        public async Task<List<Solution>> GetAllByStudentId(Guid studentId)
        {
            var solutionEntities = await _context.Solutions.
                AsNoTracking().
                Where(s => s.StudentId == studentId).
                ToListAsync();

            var solutions = solutionEntities
                .Select(MapToDomain)
                .ToList();

            return solutions;
        }

        public async Task<List<Solution>> GetAllByTutorTaskId(Guid tutorTaskId)
        {
            var solutionEntities = await _context.Solutions
                .AsNoTracking()
                .Where(s => s.TutorTaskId == tutorTaskId)
                .ToListAsync();

            var solutions = solutionEntities
                .Select(MapToDomain)
                .ToList();

            return solutions;
        }

        private static Solution MapToDomain(SolutionEntity solutionEntity)
        {
            var result = Solution.Create(
                solutionEntity.Id,
                solutionEntity.TutorTaskId,
                solutionEntity.StudentId,
                solutionEntity.AnswerText,
                solutionEntity.SubmittedAt
                );

            if(result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
