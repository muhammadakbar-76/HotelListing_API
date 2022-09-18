using HotelListing.API.Core.Dtos.HotelDtos;

namespace HotelListing.API.Core.Dtos.CountryDtos
{
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; init; }
        public List<HotelDto> Hotels { get; init; }
    }
}
