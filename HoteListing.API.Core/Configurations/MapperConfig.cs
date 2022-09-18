using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Core.Dtos.CountryDtos;
using HotelListing.API.Core.Dtos.HotelDtos;
using HotelListing.API.Core.Dtos.UserDtos;

namespace HotelListing.API.Core.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, UpdateHotelDto>().ReverseMap();
            CreateMap<Hotel, GetHotelDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
