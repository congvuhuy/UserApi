using apiUser.DAL.Model;
using Microsoft.EntityFrameworkCore;
namespace apiUser.DAL.Repository
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _context.Users.Include(u => u.PhoneNumbers).Include(u => u.Addresses).ToListAsync();

        public async Task<User> GetUserByIdAsync(int id)=>await _context.Users.Include(u => u.PhoneNumbers).Include(u => u.Addresses).FirstOrDefaultAsync(u => u.Id == id);
        

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        
            // Kiểm tra trong cơ sở dữ liệu xem có số điện thoại nào trùng lặp không,  
            // ngoại trừ của người dùng hiện tại (theo userId)  
        public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber, int userId)
        {
            
            return await _context.Users.AnyAsync(u => u.PhoneNumbers.Any(p => p.Number == phoneNumber) && u.Id != userId);
        }
        // Kiểm tra trong cơ sở dữ liệu xem có địa chỉ nào trùng lặp không,  
        // ngoại trừ của người dùng hiện tại (theo userId)  
        public async Task<bool> CheckAddressExistsAsync(Address address, int userId)
        {
            return await _context.Users.AnyAsync(u => u.Addresses.Any(a => a.Street == address.Street && a.City == address.City) && u.Id != userId);
        }
    }
}
