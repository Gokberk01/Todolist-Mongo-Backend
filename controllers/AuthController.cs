using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using api.Entities;
using api.services;
using api.shared;
using api.shared.Dtos;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace api.controllers
{
    [Route ("api/[Controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, ITokenService tokenService, JwtService jwtService)
        {
            _userService = userService;
            _tokenService = tokenService;
            _jwtService = jwtService;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDto registerDto) 
        {
            var user = new User 
            {
                UserName = registerDto.UserName,
                UserEmail = registerDto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.UserPassword)
            };

            return Created("success", await _userService.CreateAsync(user));
        }

        [HttpPost("login")]
            public async Task<ActionResult> Login(LoginDto dto)
        {
            var user = await _userService.GetByEmail(dto.UserEmail);

            if(user == null)
            {
                return BadRequest(new {message = "Invalid Credentials"});
            }

            if(!BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.UserPassword))
            {
                return BadRequest(new {message = "Invalid Credentials"});
            }

            int expirationTime = 1;
            var jwt = _jwtService.Generate(user.Id, expirationTime);

            var _refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = _refreshToken,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(3),
                Created = DateTime.UtcNow,
            };
            await _tokenService.AddRefreshToken(refreshTokenEntity);

            return Ok(new 
            { 
                token = jwt,
                refreshToken = _refreshToken             
            });
        }

        [HttpPost("logout")] 
        public async Task<ActionResult> Logout(LogoutDto refreshToken)
        {
            if (!Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
            {
                return Unauthorized();
            }

            var bearerToken = authHeader.ToString().Split(' ').Last();

            var token = _jwtService.Verify(bearerToken);

            if(token == null)
            {
                return Unauthorized();
            }

            var userId = token.Issuer;

            var RefreshTokenEntity = await _tokenService.GetRefreshToken(refreshToken.RefreshToken);

            if(RefreshTokenEntity == null || RefreshTokenEntity.UserId != userId || !await _tokenService.IsRefreshTokenValid(refreshToken.RefreshToken))
            {
                return Unauthorized();
            }

            await _tokenService.RevokeRefreshToken(refreshToken.RefreshToken);

            return Ok(new 
            {
                message = "Logout successfull", 
            });

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken(TokenRequestDto tokenRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } 

            var existingRefreshToken = await _tokenService.GetRefreshToken(tokenRequestDto.RefreshToken);

            if(existingRefreshToken == null || !existingRefreshToken.IsActive)
            {
                return Unauthorized(new { message = "Invalid refresh token"});
            }

            var token = _jwtService.Verify(tokenRequestDto.AccessToken,false); 

            if(token == null)
            {
                return Unauthorized(new { message = "Invalid access token"});
            }

            var userId = token.Issuer;

            if(existingRefreshToken.UserId != userId)
            {
                return Unauthorized(new { message = "Invalid refresh token for this user"});
            }

            int expirationTime = 1;
            var newAccessToken = _jwtService.Generate(userId,expirationTime);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            await _tokenService.RevokeRefreshToken(tokenRequestDto.RefreshToken);

            var newRefreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(3), // 3 gün geçerlilik süresi
                Created = DateTime.UtcNow,
            };
            await _tokenService.AddRefreshToken(newRefreshTokenEntity);

            return Ok(new 
            { 
                token = newAccessToken,
                refreshToken = newRefreshToken             
            });
        }    
    }
}