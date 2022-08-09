namespace HotelListing.API.Dtos.Hotel
{
    public class HotelDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Address { get; init; }
        public double Rating { get; init; }
        public int CountryId { get; init; }
    }
}
