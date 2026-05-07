using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class SubjectsRepository : ISubjectsRepository
    {
        private readonly TeachlyDbContext _context;

        public SubjectsRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(Subject subject)
        {
            var subjectEntity = new SubjectEntity()
            {
                Id = subject.Id,
                Name = subject.Name,
            };

            await _context.Subjects.AddAsync(subjectEntity);
            await _context.SaveChangesAsync();
        }
        public async Task<Subject?> GetById(Guid id)
        {
            var subjectEntity = await _context.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if(subjectEntity is null)
            {
                return null;
            }

            return MapToDomain(subjectEntity);
        }

        public async Task<Subject?> GetByName(string name)
        {
            var subjectEntity = await _context.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Name == name);

            if (subjectEntity is null)
            {
                return null;
            }

            return MapToDomain(subjectEntity);
        }

        public async Task<List<Subject>> GetAll()
        {
            var subjectEntitys = await _context.Subjects
                .AsNoTracking()
                .ToListAsync();

            var subjects = subjectEntitys
                .Select(MapToDomain)
                .ToList();

            return subjects;
        }

        private static Subject MapToDomain(SubjectEntity subjectEntity)
        {
            var result = Subject.Create(subjectEntity.Id, subjectEntity.Name);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
