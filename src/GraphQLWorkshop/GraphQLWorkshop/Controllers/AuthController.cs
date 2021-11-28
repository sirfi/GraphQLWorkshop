using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GraphQLWorkshop.Data.Entities;
using GraphQLWorkshop.ViewModels.Login;
using GraphQLWorkshop.ViewModels.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLWorkshop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<LoginResponseDto> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid) return new LoginResponseDto { IsSuccess = false, Message = string.Join(" ", ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))) };
            var user = await _signInManager.UserManager.FindByNameAsync(request.UserName);
            if (user == null) return new LoginResponseDto { IsSuccess = false, Message = "Login failed" };
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!result.Succeeded) return new LoginResponseDto { IsSuccess = false, Message = "Login failed" };
            var token = await GenerateJwtAsync(user);
            return new LoginResponseDto { IsSuccess = true, User = new UserDto { UserName = user.UserName, Email = user.Email, AccessToken = token } };
        }

        private async Task<string> GenerateJwtAsync(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                await GetValidClaimsAsync(user),
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetValidClaimsAsync(AppUser user)
        {
            IdentityOptions options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id),
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName)
            };
            var userClaims = await _signInManager.UserManager.GetClaimsAsync(user);
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);
            claims.AddRange(userClaims);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        [HttpPost]
        public async Task<RegisterResponseDto> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid) return new RegisterResponseDto { IsSuccess = false, Message = string.Join(" ", ModelState.SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage))) };
            var user = new AppUser {UserName = request.UserName, Email = request.Email};
            var result = await _signInManager.UserManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return new RegisterResponseDto { IsSuccess = false, Message = "Register failed" };
            var token = await GenerateJwtAsync(user);
            return new RegisterResponseDto { IsSuccess = true, User = new UserDto { UserName = user.UserName, Email = user.Email, AccessToken = token } };
        }
    }
}
