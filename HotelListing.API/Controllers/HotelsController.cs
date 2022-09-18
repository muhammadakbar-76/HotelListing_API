using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Core.Interfaces;
using AutoMapper;
using HotelListing.API.Core.Dtos.HotelDtos;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Data.Constants;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _context;
        private readonly IMapper _mapper;

        public HotelsController(IHotelsRepository context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetHotelDto>>> GetHotels()
        {
            var hotel = await _context.GetAllAsync();
            var record = _mapper.Map<List<GetHotelDto>>(hotel);
            return Ok(record);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _context.GetDetails(id);

            if (hotel == null)
            {
                return NotFound();
            }

            var record = _mapper.Map<HotelDto>(hotel);
            record.CountryName = hotel.Country.Name;

            return Ok(record);
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, UpdateHotelDto updatedHotel)
        {
            if (id != updatedHotel.Id)
            {
                return BadRequest();
            }

            var hotel = await _context.GetAsync(id);

            if (hotel is null)
            {
                return NotFound();
            }

            _mapper.Map(updatedHotel, hotel);

            try
            {
                await _context.UpdateAsync(hotel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetHotelDto>> PostHotel(CreateHotelDto hotel)
        {
            var newHotel = _mapper.Map<Hotel>(hotel);
            var record = await _context.AddAsync(newHotel);

            return CreatedAtAction(nameof(GetHotel), new { id = record.Id }, record);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Const.Roles.Administrator)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var hotel = await _context.GetAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            await _context.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _context.Exists(id);
        }
    }
}
