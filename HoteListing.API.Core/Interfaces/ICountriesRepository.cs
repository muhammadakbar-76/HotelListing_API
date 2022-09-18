using HotelListing.API.Core.Dtos.CountryDtos;
using HotelListing.API.Data;

namespace HotelListing.API.Core.Interfaces
{
    public interface ICountriesRepository : IGenericRepository<Country>
    {
        Task<CountryDto> GetDetails(int id);
    }
}
