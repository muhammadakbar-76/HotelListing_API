using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Dtos.HotelDtos
{
    public class CreateHotelDto : BaseHotelDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CountryId { get; init; }
    }
}
