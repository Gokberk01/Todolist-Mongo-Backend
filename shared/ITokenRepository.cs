using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;

namespace api.shared
{
    public interface ITokenRepository
    {
        Task AddToken(RefreshToken refreshToken);
        Task<RefreshToken> GetToken(string token);
        Task UpdateRefreshToken(RefreshToken refreshToken);
        Task RevokeRefreshToken(string token);
        Task<bool> IsRefreshTokenValid(string token);    
    }
}