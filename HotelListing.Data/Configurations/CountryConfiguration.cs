﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
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
        }
    }
}
