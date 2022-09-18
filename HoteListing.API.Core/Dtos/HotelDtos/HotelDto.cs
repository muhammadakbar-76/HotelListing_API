namespace HotelListing.API.Core.Dtos.HotelDtos
{
    public class HotelDto : BaseHotelDto
    {
        public int Id { get; init; }
        public string CountryName { get; set; }
    }
}
