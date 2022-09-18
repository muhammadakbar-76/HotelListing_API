using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Kartika",
                    Address = "Kotabaru, Kalsel",
                    Rating = 5,
                    CountryId = 1,
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Onsen",
                    Address = "Tokyo",
                    Rating = 4.7,
                    CountryId = 3
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Uraaa",
                    Address = "Moskow",
                    Rating = 4.8,
                    CountryId = 2
                });
        }
    }
}
