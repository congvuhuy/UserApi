using apiUser.DAL.Repository;
using apiUser.DAL.Model;
using System.Numerics;
using System.Net;
using Microsoft.EntityFrameworkCore;
using apiUser.DAL;

namespace apiUser.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public UserService(IUserRepository userRepository, AppDbContext context)
        {
            _userRepository = userRepository;
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> GetUsersAsync() => await _userRepository.GetAllUsersAsync();

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task CreateUserAsync(User user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (user.PhoneNumbers != null)
            {
                foreach (var phoneNumber in user.PhoneNumbers)
                {
                    var isAddressExists = await _userRepository.CheckPhoneNumberExistsAsync(phoneNumber.Number, user.Id);
                    if (isAddressExists)
                    {
                    
                        throw new InvalidOperationException("Phone number already exists.");

                    }
                }
            }

            if (user.Addresses != null)
            {
                foreach (var address in user.Addresses)
                {

                    var isAddressExists = await _userRepository.CheckAddressExistsAsync(address, user.Id);
                    if (isAddressExists)
                    {
                    
                        throw new InvalidOperationException("Address already exists.");

                    }
                }
            } 
                await _userRepository.CreateUserAsync(user);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
           
        }

        public async Task UpdateUserAsync(User user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(user.Id);
                if (existingUser == null)
                {
                    throw new KeyNotFoundException("User not found");
                }
                existingUser.PhoneNumbers.Clear();
                existingUser.Addresses.Clear();
                if (user.PhoneNumbers != null)
                {
                    foreach (var phoneNumber in user.PhoneNumbers)
                    {
                        var isAddressExists = await _userRepository.CheckPhoneNumberExistsAsync(phoneNumber.Number, user.Id);
                        if (!isAddressExists)
                        {
                            phoneNumber.UserId = user.Id;
                            existingUser.PhoneNumbers.Add(phoneNumber);
                        }
                        else
                        {
                            throw new InvalidOperationException("Phone number already exists.");

                        }
                    }
                }

                if (user.Addresses != null)
                {
                    foreach (var address in user.Addresses)
                    {

                        var isAddressExists = await _userRepository.CheckAddressExistsAsync(address, user.Id);
                        if (!isAddressExists)
                        {
                            address.UserId = existingUser.Id;
                            existingUser.Addresses.Add(address); 
                        }
                        else
                        {
                            throw new InvalidOperationException("Address already exists.");

                        }
                    }
                }
                await _userRepository.UpdateUserAsync(existingUser);
                await transaction.CommitAsync();
            }
            catch
            {
                    await transaction.RollbackAsync();
                    throw;
            }

}

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

    }

}
