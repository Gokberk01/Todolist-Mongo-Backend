using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using api.shared;
using MongoDB.Driver;

namespace api.repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoDatabase database)
        {   
            _userCollection = database.GetCollection<User>("UserItems");
        }

        public async Task CreateAsync(User User)
        {   
            await _userCollection.InsertOneAsync(User);
        }

        public async Task<User> GetByEmail(string userEmail)
        {   
            return await _userCollection.Find(x => x.UserEmail == userEmail).FirstOrDefaultAsync();
        }

        public async Task<User> GetById(string userId)
        {   
                return await _userCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
        }
    }
}