using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using api.services;
using api.shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace api.controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public UserController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            try
            {

                // Authorization başlığından JWT token'ı al
                if (!Request.Headers.TryGetValue("Authorization", out StringValues authHeader))
                {
                    return Unauthorized();
                }

                var bearerToken = authHeader.ToString().Split(' ').Last();

                var token = _jwtService.Verify(bearerToken);

                // UserId'ye göre kullanıcı bilgilerini al
                var user = await _userService.GetById(token.Issuer);

                return Ok(user);
            }
             catch (Exception)
            {
                
                return Unauthorized();
            }


        }
    }
}