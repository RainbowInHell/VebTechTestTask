namespace VebTechTestTask.BLL.Services.Interfaces
{
    using DAL.Entities;

    public interface IUserService
    {
        public Task<List<User>> GetUsersAsync(
            string? sortBy,
            bool descending,
            string? filter,
            int page,
            int pageSize);

        public Task<User> GetUserByIdAsync(int id);

        public Task<User> GetUserByEmailAsync(string email);

        public Task<List<Role>> GetRolesByUserIdAsync(int id);

        public Task AssignRoleToUserAsync(int userId, int roleId);

        public Task CreateUserAsync(User userToCreate);

        public Task UpdateUserAsync(User userToUpdate);

        public Task DeleteUserByIdAsync(int id);
    }
}