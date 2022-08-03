using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Indonesia",
                    ShortName = "ID"
                },
                new Country
                {
                    Id = 2,
                    Name = "Russia",
                    ShortName = "RU"
                },
                new Country
                {
                    Id = 3,
                    Name = "Japan",
                    ShortName = "JP"
                });
            modelBuilder.Entity<Hotel>().HasData(
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
