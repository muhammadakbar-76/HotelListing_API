using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Dtos.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; init; }
        public string ShortName { get; init; }
    }
}
