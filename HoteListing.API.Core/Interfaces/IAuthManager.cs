using HotelListing.API.Core.Dtos.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Core.Interfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(UserDto userDto);
        Task<IEnumerable<IdentityError>> RegisterAdmin(UserDto userDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
        Task<string> CreateRefreshToken();

    }
}
