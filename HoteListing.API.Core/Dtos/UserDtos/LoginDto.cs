using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Dtos.UserDtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Password must from 8 to 15 range", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
