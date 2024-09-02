using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.shared
{
    public interface IUserService
    {
        Task<User> CreateAsync(User newUser);
        Task<User> GetByEmail(string userEmail);
        Task<User> GetById(string userId);
    }
}