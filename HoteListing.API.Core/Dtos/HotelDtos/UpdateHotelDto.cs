namespace HotelListing.API.Core.Dtos.HotelDtos
{
    public class UpdateHotelDto : BaseHotelDto
    {
        public int Id { get; init; }
        public int CountryId { get; init; }
    }
}
