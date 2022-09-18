using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Dtos.UserDtos
{
    public class UserDto : LoginDto
    {
        [Required]
        public string FirstName { get; init; }
        [Required]
        public string LastName { get; init; }
    }
}
