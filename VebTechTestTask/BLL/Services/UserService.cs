namespace VebTechTestTask.BLL.Services
{
    using System.Data;
    using System.Linq.Expressions;

    using DAL.Entities;
    using DAL.UnitOfWork.Interfaces;

    using Interfaces;

    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private const string UserNotFoundErrorMessage = "User not found.";

        private protected IUnitOfWork unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<User>> GetUsersAsync(
            string? sortBy,
            bool descending,
            string? filter,
            int page,
            int pageSize)
        {
            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.Snapshot))
            {
                var usersQuery = unitOfWork.UserRepository.GetAsQueryable(true);

                if (!string.IsNullOrEmpty(filter))
                {
                    usersQuery = usersQuery.Where(u =>
                        u.Name.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        u.Id.ToString().Contains(filter));
                }

                if (!string.IsNullOrEmpty(sortBy))
                {
                    usersQuery = descending
                        ? usersQuery.OrderByDescending(GetSortExpression(sortBy))
                        : usersQuery.OrderBy(GetSortExpression(sortBy));
                }

                var users = await usersQuery
                    .Include(x => x.UserLinks)
                    .ThenInclude(x => x.Role)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return users;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.Snapshot))
            {
                return await unitOfWork.UserRepository
                    .GetAsQueryable(true)
                    .Where(x => x.Id == userId)
                    .Include(x => x.UserLinks)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.Snapshot))
            {
                var user = await unitOfWork.UserRepository
                    .GetAsQueryable(true)
                    .Where(x => x.Email == email)
                    .FirstOrDefaultAsync();

                return user;
            }
        }

        public async Task<List<Role>> GetRolesByUserIdAsync(int id)
        {
            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.Snapshot))
            {
                var roles = await unitOfWork.UserLinkRepository
                    .GetAsQueryable(true)
                    .Include(u => u.Role)
                    .Where(x => x.UserId == id)
                    .Select(x => x.Role)
                    .ToListAsync();

                return roles;
            }
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            var existingUser = await GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new ArgumentException(UserNotFoundErrorMessage);
            }

            using (var transaction = unitOfWork.BeginTransaction(IsolationLevel.Snapshot))
            {
                var existingRole = await unitOfWork.RoleRepository
                    .GetAsQueryable()
                    .Include(x => x.UserLinks)
                    .Where(x => x.Id == roleId)
                    .FirstOrDefaultAsync();

                if (existingRole == null)
                {
                    throw new ArgumentException("Role with provided Id doesn't exist.");
                }

                if (existingRole.UserLinks.Any(x => x.UserId == userId))
                {
                    throw new ArgumentException("User alredy has provided role.");
                }
            }

            var newUserLink = new UserLink()
            {
                UserId = userId,
                RoleId = roleId
            };

            using (var transaction = unitOfWork.BeginTransaction())
            {
                unitOfWork.UserLinkRepository.Add(newUserLink);

                transaction.Commit();
                await unitOfWork.SaveAsync();
            }
        }

        public async Task CreateUserAsync(User userToCreate)
        {
            var user = await unitOfWork.UserRepository
                .GetAsQueryable()
                .Where(x => x.Email == userToCreate.Email)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                throw new ArgumentException("User alredy exist with provided email.");
            }

            using (var transaction = unitOfWork.BeginTransaction())
            {
                unitOfWork.UserRepository.Add(userToCreate);

                transaction.Commit();
                await unitOfWork.SaveAsync();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await GetUserByIdAsync(user.Id);

            if (user == null)
            {
                throw new ArgumentException(UserNotFoundErrorMessage);
            }

            existingUser.Name = user.Name;
            existingUser.Age = user.Age;
            existingUser.Email = user.Email;

            using (var transaction = unitOfWork.BeginTransaction())
            {
                unitOfWork.UserRepository.Update(existingUser);

                transaction.Commit();
                await unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteUserByIdAsync(int userId)
        {
            var existingUser = await GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                throw new ArgumentException(UserNotFoundErrorMessage);
            }

            using (var transaction = unitOfWork.BeginTransaction())
            {
                unitOfWork.UserRepository.Delete(userId);

                transaction.Commit();
                await unitOfWork.SaveAsync();
            }
        }

        private static Expression<Func<User, object>> GetSortExpression(string sortBy)
        {
            return sortBy switch
            {
                "Name" => u => u.Name,
                "Email" => u => u.Email,
                "Age" => u => u.Age,
                _ => u => u.Id,
            };
        }
    }
}