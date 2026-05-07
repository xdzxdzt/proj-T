using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class TutorTasksRepository : ITutorTasksRepository
    {
        private readonly TeachlyDbContext _context;

        public TutorTasksRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(TutorTask task)
        {
            var taskEntity = new TutorTaskEntity()
            { 
                Id = task.Id,
                LessonPackageId = task.LessonPackageId,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = task.CreatedAt,
                ClosedAt = task.ClosedAt
            };

            await _context.TutorTasks.AddAsync(taskEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<TutorTask?> GetById(Guid id)
        {
            var taskEntity = await _context.TutorTasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if(taskEntity is null)
            {
                return null;
            }

            return MapToDomain(taskEntity);
        }

        public async Task<List<TutorTask>> GetByLessonPackageId(Guid lessonPackageId)
        {
            var taskEntities = await _context.TutorTasks
                .AsNoTracking()
                .Where(t => t.LessonPackageId == lessonPackageId)
                .ToListAsync();

            var tasks = taskEntities
                .Select(MapToDomain)
                .ToList();

            return tasks;
        }

        public async Task Update(TutorTask task)
        {
            var rowsAffected = await _context.TutorTasks
                .Where(t => t.Id == task.Id)
                .ExecuteUpdateAsync(t => t
                    .SetProperty(x => x.Title, task.Title)
                    .SetProperty(x => x.Description, task.Description)
                    .SetProperty(x => x.ClosedAt, task.ClosedAt));

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Задание не найдено");
            }
        }

        private static TutorTask MapToDomain(TutorTaskEntity taskEntity)
        {
            var result = TutorTask.Create(
                taskEntity.Id,
                taskEntity.LessonPackageId,
                taskEntity.Title,
                taskEntity.Description,
                taskEntity.CreatedAt,
                taskEntity.ClosedAt);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            return result.Value;
        }
    }
}
