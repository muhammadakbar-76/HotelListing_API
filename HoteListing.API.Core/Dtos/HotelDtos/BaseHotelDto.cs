using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Dtos.HotelDtos
{
    public abstract class BaseHotelDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string Address { get; init; }
        public double? Rating { get; init; }
    }
}
