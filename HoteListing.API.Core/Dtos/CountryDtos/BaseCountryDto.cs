using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Dtos.CountryDtos
{
    public abstract class BaseCountryDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; init; }
        public string ShortName { get; init; }
    }
}
