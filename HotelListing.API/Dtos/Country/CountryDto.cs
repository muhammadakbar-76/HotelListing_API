using HotelListing.API.Dtos.Hotel;

namespace HotelListing.API.Dtos.Country
{
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; init; }
        public List<HotelDto> Hotels { get; init; }
    }
}
