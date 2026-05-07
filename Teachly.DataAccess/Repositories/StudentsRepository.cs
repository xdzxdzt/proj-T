using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly TeachlyDbContext _context;

        public StudentsRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(Student student)
        {
            var studentEntity = new StudentEntity()
            {
                Id = student.Id,
                UserId = student.UserId,
                InstitutionId = student.InstitutionId,
                EducationLevel = student.EducationLevel,
                ParentPhone = student.ParentPhone
            };

            await _context.Students.AddAsync(studentEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<Student?> GetById(Guid id)
        {
            var studentEntity = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (studentEntity is null)
            {
                return null;
            }

            return MapToDomain(studentEntity);
        }

        public async Task<Student?> GetByUserId(Guid userId)
        {
            var studentEntity = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (studentEntity is null)
            {
                return null;
            }

            return MapToDomain(studentEntity);
        }

        public async Task<bool> ExistsByUserId(Guid userId)
        {
            return await _context.Students
                .AsNoTracking()
                .AnyAsync(s => s.UserId == userId);
        }

        private static Student MapToDomain(StudentEntity studentEntity)
        {
            var result = Student.Create(
                studentEntity.Id,
                studentEntity.UserId,
                studentEntity.InstitutionId,
                studentEntity.EducationLevel,
                studentEntity.ParentPhone);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
