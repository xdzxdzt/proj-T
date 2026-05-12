using Microsoft.EntityFrameworkCore;
using Teachly.Application.Interfaces.Repositories;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly TeachlyDbContext _context;

        public UsersRepository(TeachlyDbContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            var userEntity = new UserEntity()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                AvatarUrl = user.AvatarUrl,
                Role = user.Role,
                RegisteredAt = user.RegisteredAt
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetById(Guid id)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity is null)
            {
                return null;
            }

            return MapToDomain(userEntity);
        }

        public async Task<User?> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (userEntity is null)
            {
                return null;
            }

            return MapToDomain(userEntity);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email);
        }

        private static User MapToDomain(UserEntity userEntity)
        {
            var result = User.Create(
                userEntity.Id,
                userEntity.UserName,
                userEntity.FirstName,
                userEntity.LastName,
                userEntity.Age,
                userEntity.Email,
                userEntity.PasswordHash,
                userEntity.Role,
                userEntity.RegisteredAt);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Error);
            }

            if (!string.IsNullOrWhiteSpace(userEntity.AvatarUrl))
            {
                var avatarResult = result.Value.SetAvatarUrl(userEntity.AvatarUrl);

                if (avatarResult.IsFailure)
                {
                    throw new InvalidOperationException(avatarResult.Error);
                }
            }

            return result.Value;
        }

        public async Task Update(User user)
        {
            var rowsAffected = await _context.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(x => x.UserName, user.UserName)
                    .SetProperty(x => x.FirstName, user.FirstName)
                    .SetProperty(x => x.LastName, user.LastName)
                    .SetProperty(x => x.Age, user.Age)
                    .SetProperty(x => x.Email, user.Email)
                    .SetProperty(x => x.PasswordHash, user.PasswordHash)
                    .SetProperty(x => x.AvatarUrl, user.AvatarUrl)
                    .SetProperty(x => x.Role, user.Role));

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException("Пользователь не найден");
            }
        }
    }
}
