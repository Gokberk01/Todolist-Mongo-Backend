using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using api.shared;
using MongoDB.Driver;

namespace api.repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _tokenCollection;

        public TokenRepository(IMongoDatabase database)
        {
            _tokenCollection = database.GetCollection<RefreshToken>("TokenItems");
        }

        public async Task AddToken(RefreshToken refreshToken)
        {
            await _tokenCollection.InsertOneAsync(refreshToken);
        }

        public async Task<RefreshToken> GetToken(string token)
        {
            return await _tokenCollection.Find(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task UpdateRefreshToken(RefreshToken refreshToken)
        {
            await _tokenCollection.ReplaceOneAsync(x => x.Id == refreshToken.Id ,refreshToken);
        }

        public async Task RevokeRefreshToken(string token)
        {
            
            var refreshToken = await GetToken(token);
            if (refreshToken != null)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await UpdateRefreshToken(refreshToken);
            }
        }

        public async Task<bool> IsRefreshTokenValid(string token)
        {
            var refreshToken = await GetToken(token);
            return refreshToken != null && refreshToken.IsActive;
        }
    }
}