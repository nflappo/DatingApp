using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;

namespace DatingApp.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Properties
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        #endregion
        #region Constructor
        public AuthController(IAuthRepository authRepo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _authRepository = authRepo;
            _config = config;
        }

        #endregion

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userToRegister)
        {
            //validate model
            userToRegister.UserName = userToRegister.UserName.ToLower();
            if(await _authRepository.UserExists(userToRegister.UserName))
            {
                return BadRequest("user already exists");
            }
            var userCreated = _mapper.Map<User>(userToRegister);
            var user = await _authRepository.Register(userCreated, userToRegister.Password);
            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            return CreatedAtRoute("GetUser", new {controller = "Users", id = user.Id} ,userToReturn);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userToLogin)
        {
            var userRepo = await _authRepository.Login(userToLogin.UserName.ToLower(), userToLogin.Password);
            if(userRepo == null)
            {
                return Unauthorized();
            }
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, userRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userRepo.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(descriptor);
            var user = _mapper.Map<UserForListDTO>(userRepo);

            return Ok(
            new 
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }
}