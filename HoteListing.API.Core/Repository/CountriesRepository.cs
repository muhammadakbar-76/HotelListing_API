using HotelListing.API.Core.Interfaces;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Dtos.CountryDtos;
using HotelListing.API.Core.Exceptions;

namespace HotelListing.API.Core.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListingDbContext _context;

        private readonly IMapper _mapper;

        public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CountryDto> GetDetails(int id)
        {
            var country = await _context.Countries
               .Include(q => q.Hotels)
               .ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(h => h.Id == id);

            if (country is null)
            {
                throw new NotFoundException(nameof(GetDetails), id);
            }

            return country;
        }
    }
}
