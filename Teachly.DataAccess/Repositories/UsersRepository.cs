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

            return result.Value;
        }
    }
}
