using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using api.shared;

namespace api.services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateAsync(User newUser)
        {
            await _userRepository.CreateAsync(newUser);
            return newUser;
        }

        public async Task<User> GetByEmail(string userEmail)
        {           
            var user = await _userRepository.GetByEmail(userEmail);
            return user;
        }

        public async Task<User> GetById(string userId)
        {           
            var user = await _userRepository.GetById(userId);
            return user;
        }
    }
}