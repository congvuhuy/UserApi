using apiUser.DAL.Model;
namespace apiUser.DAL.Repository

{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> CheckAddressExistsAsync(Address address, int userId);
        Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber, int userId);
    }
}

