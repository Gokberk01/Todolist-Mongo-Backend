using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using api.Entities;
using api.shared;

namespace api.services
{
    public class TokenService :ITokenService
    {
        private readonly ITokenRepository _tokenRepository;

        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public async Task AddRefreshToken(RefreshToken refreshToken)
        {
            await _tokenRepository.AddToken(refreshToken);
        }

        public async Task<RefreshToken> GetRefreshToken(string token)
        {
            return await _tokenRepository.GetToken(token);
        }

        public async Task RevokeRefreshToken(string token)
        {
            var refreshToken = await _tokenRepository.GetToken(token);
            if(refreshToken.Revoked == null)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await _tokenRepository.UpdateRefreshToken(refreshToken);
            }
        }

        public async Task<bool> IsRefreshTokenValid(string token)
        {
            return await _tokenRepository.IsRefreshTokenValid(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


    }
}