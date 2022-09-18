namespace HotelListing.API.Core.Dtos.UserDtos
{
    public class AuthResponseDto
    {
        public string UserId { get; init; }
        public string Token { get; init; }
#nullable enable
        public string? RefreshToken { get; set; }
    }
}
