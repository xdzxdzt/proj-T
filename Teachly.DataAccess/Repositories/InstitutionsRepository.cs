using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class InstitutionsRepository : IInstitutionsRepository
    {
        private readonly TeachlyDbContext _context;

        public InstitutionsRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(Institution institution)
        {
            var institutionEntity = new InstitutionEntity()
            {
                Id = institution.Id,
                Type = institution.Type,
                Name = institution.Name,
                City = institution.City
            };

            await _context.Institutions.AddAsync(institutionEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Institution?> GetById(Guid id)
        {
            var institutionEntity = await _context.Institutions
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (institutionEntity == null)
            {
                return null;
            }

            return MapToDomain(institutionEntity);
        }

        public async Task<List<Institution>> GetAll()
        {
            var institutionEntities = await _context.Institutions
                .AsNoTracking()
                .ToListAsync();

            var institutions = institutionEntities
                .Select(MapToDomain)
                .ToList();

            return institutions;
        }

        private static Institution MapToDomain(InstitutionEntity institutionEntity)
        {
            var result = Institution.Create(
                institutionEntity.Id,
                institutionEntity.Type,
                institutionEntity.Name,
                institutionEntity.City);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
