using Application.DTOs.Auth;
using Application.Services;
using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.ImpServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _user;
        private readonly IConfiguration _config;

        public AuthService(UserManager<ApplicationUser> user, IConfiguration config)
        {
            _user = user;
            _config = config;
        }

        public async Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginDto model)
        {
            try
            {
                var user = await _user.FindByEmailAsync(model.Email);
                if (user == null)
                    return BaseResponse<AuthResponseDto>.FailureResult("can't find email");
                var password = await _user.CheckPasswordAsync(user, model.Password);
                if (!password)
                    return BaseResponse<AuthResponseDto>.FailureResult("Invalid Credintials");

                var token = await GenerateJWTToken(user);

                return BaseResponse<AuthResponseDto>.SuccessResult(token, "Login successful");
            }
            catch (Exception ex)
            {
                return BaseResponse<AuthResponseDto>.FailureResult($"Error: {ex.Message}");
            }
        }

        public async Task<BaseResponse<AuthResponseDto>> RegisterAsync(RegisterDto model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.FullName,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.Phone,
                    CreatedAt = DateTime.UtcNow
                };
                var result = await _user.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return BaseResponse<AuthResponseDto>.FailureResult(
                        "Registration failed",
                        result.Errors.Select(e => e.Description).ToList());
                }
                await _user.AddToRoleAsync(user, "Student"); var token = await GenerateJWTToken(user);
                return BaseResponse<AuthResponseDto>.SuccessResult(token, "Registeration Successful");
            }
            catch (Exception ex)
            {
                return BaseResponse<AuthResponseDto>.FailureResult($"Error: {ex.Message}");
            }
        }
        private async Task<AuthResponseDto> GenerateJWTToken(ApplicationUser user)
        {
            var roles = await _user.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddDays(7);
            var token = new JwtSecurityToken(
               issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
                );
            return new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = roles.FirstOrDefault(),
                ExpiresAt = expiresAt,

            };
        }
    }
}
