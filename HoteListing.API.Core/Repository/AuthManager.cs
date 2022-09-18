using AutoMapper;
using HotelListing.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HotelListing.API.Data.Constants;
using HotelListing.API.Core.Interfaces;
using HotelListing.API.Core.Dtos.UserDtos;

namespace HotelListing.API.Core.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private User _user;

        public AuthManager(IMapper mapper, UserManager<User> userManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, Const.Token.LoginProvider, Const.Token.TokenName);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, Const.Token.LoginProvider, Const.Token.TokenName);
            await _userManager.SetAuthenticationTokenAsync(_user, Const.Token.LoginProvider, Const.Token.TokenName, newRefreshToken);
            return newRefreshToken;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _logger.LogInformation($"Looking for user with email {loginDto.Email}");
            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

            if (_user == null || isValidUser == false)
            {
                _logger.LogWarning($"User with email {loginDto.Email} was not found or invalid password");
                return null;
            }

            var token = await GenerateToken();
            var refreshToken = await CreateRefreshToken();
            _logger.LogInformation($"Token generated for user with email {loginDto.Email} | Token {token}");
            return new AuthResponseDto
            {
                UserId = _user.Id,
                Token = token,
                RefreshToken = refreshToken,
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(UserDto userDto)
        {
            _user = _mapper.Map<User>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, Const.Roles.User);
            }

            return result.Errors;
        }

        public async Task<IEnumerable<IdentityError>> RegisterAdmin(UserDto userDto)
        {
            _user = _mapper.Map<User>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user);

            if (result.Succeeded) await _userManager.AddToRoleAsync(_user, Const.Roles.Administrator);

            return result.Errors;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            _user = await _userManager.FindByNameAsync(username);

            if (_user is null || _user.Id != request.UserId) return null;

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, Const.Token.LoginProvider, Const.Token.TokenName, request.RefreshToken);

            if(isValidRefreshToken)
            {
                var refreshToken = await CreateRefreshToken();
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    RefreshToken = refreshToken,
                    UserId = _user.Id,
                    Token = token,
                };
            }
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //jti just like nonche
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
