using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class LessonPackagesRepository : ILessonPackagesRepository
    {
        private readonly TeachlyDbContext _context;

        public LessonPackagesRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(LessonPackage lessonPackage)
        {
            var lessonPackageEntity = new LessonPackageEntity()
            {
                Id = lessonPackage.Id,
                StudentId = lessonPackage.StudentId,
                TutorSubjectId = lessonPackage.TutorSubjectId,
                TotalLessons = lessonPackage.TotalLessons,
                RemainingLessons = lessonPackage.RemainingLessons,
                PricePerLesson = lessonPackage.PricePerLesson,
                TotalPrice = lessonPackage.TotalPrice,
                PurchasedAt = lessonPackage.PurchasedAt,
                CompletedAt = lessonPackage.CompletedAt
            };

            await _context.LessonPackages.AddAsync(lessonPackageEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<LessonPackage?> GetById(Guid id)
        {
            var lessonPackageEntity = await _context.LessonPackages
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lessonPackageEntity is null)
            {
                return null;
            }

            return MapToDomain(lessonPackageEntity);
        }

        public async Task<List<LessonPackage>> GetByStudentId(Guid studentId)
        {
            var lessonPackageEntities = await _context.LessonPackages
                .AsNoTracking()
                .Where(l => l.StudentId == studentId)
                .ToListAsync();

            return lessonPackageEntities
                .Select(MapToDomain)
                .ToList();
        }

        public async Task<List<LessonPackage>> GetByTutorId(Guid tutorId)
        {
            var lessonPackageEntities = await _context.LessonPackages
                .AsNoTracking()
                .Where(l => l.TutorSubject.TutorId == tutorId)
                .ToListAsync();

            return lessonPackageEntities
                .Select(MapToDomain)
                .ToList();
        }

        public async Task Update(LessonPackage lessonPackage)
        {
            var rowsAffected = await _context.LessonPackages
                .Where(l => l.Id == lessonPackage.Id)
                .ExecuteUpdateAsync(l => l
                    .SetProperty(x => x.TotalLessons, lessonPackage.TotalLessons)
                    .SetProperty(x => x.RemainingLessons, lessonPackage.RemainingLessons)
                    .SetProperty(x => x.PricePerLesson, lessonPackage.PricePerLesson)
                    .SetProperty(x => x.TotalPrice, lessonPackage.TotalPrice)
                    .SetProperty(x => x.CompletedAt, lessonPackage.CompletedAt));

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Пакет занятий не найден");
            }
        }

        private static LessonPackage MapToDomain(LessonPackageEntity lessonPackageEntity)
        {
            var result = LessonPackage.Create(
                lessonPackageEntity.Id,
                lessonPackageEntity.StudentId,
                lessonPackageEntity.TutorSubjectId,
                lessonPackageEntity.TotalLessons,
                lessonPackageEntity.PricePerLesson,
                lessonPackageEntity.RemainingLessons,
                lessonPackageEntity.TotalPrice,
                lessonPackageEntity.PurchasedAt,
                lessonPackageEntity.CompletedAt);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
