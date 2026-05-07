using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class TutorSubjectsRepository : ITutorSubjectsRepository
    {
        private readonly TeachlyDbContext _context;

        public TutorSubjectsRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(TutorSubject tutorSubject)
        {
            var tutorSubjectEntity = new TutorSubjectEntity()
            {
                Id = tutorSubject.Id,
                TutorId = tutorSubject.TutorId,
                SubjectId = tutorSubject.SubjectId,
                PricePerHour = tutorSubject.PricePerHour
            };

            await _context.TutorSubjects.AddAsync(tutorSubjectEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<TutorSubject?> GetById(Guid id)
        {
            var tutorEntity = await _context.TutorSubjects
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if(tutorEntity is null)
            {
                return null;
            }

            return MapToDomain(tutorEntity);
        }

        public async Task<List<TutorSubject>> GetAllByTutorId(Guid tutorId)
        {
            var tutorSubjectEntities = await _context.TutorSubjects
                .AsNoTracking()
                .Where(t => t.TutorId == tutorId)
                .ToListAsync();

            var tutorSubject = tutorSubjectEntities
                .Select(MapToDomain)
                .ToList();

            return tutorSubject;
        }

        public async Task<bool> Exists(Guid tutorId,Guid subjectId)
        {
            return await _context.TutorSubjects
                .Where(t => t.TutorId == tutorId && t.SubjectId == subjectId)
                .AnyAsync();
        }

        private static TutorSubject MapToDomain(TutorSubjectEntity tutorSubjectEntity)
        {
            var result = TutorSubject.Create(
                tutorSubjectEntity.Id,
                tutorSubjectEntity.TutorId,
                tutorSubjectEntity.SubjectId,
                tutorSubjectEntity.PricePerHour);

            if(result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
