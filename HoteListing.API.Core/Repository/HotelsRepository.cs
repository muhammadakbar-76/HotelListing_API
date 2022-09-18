using HotelListing.API.Core.Interfaces;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace HotelListing.API.Core.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        private readonly HotelListingDbContext _context;
        public HotelsRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }
        public async Task<Hotel> GetDetails(int id)
        {
            return await _context.Hotels
                .Include(c => c.Country)
                .FirstOrDefaultAsync(h => h.Id == id);
        }
    }
}
