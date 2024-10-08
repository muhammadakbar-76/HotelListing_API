﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.API.Data;
using HotelListing.API.Core.Dtos.CountryDtos;
using AutoMapper;
using HotelListing.API.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HotelListing.API.Data.Constants;
using HotelListing.API.Core.Exceptions;

namespace HotelListing.API.Controllers
{
    [Route("api/v{version:apiVersion}/countries")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository _countriesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(ICountriesRepository countryRepo, IMapper mapper, ILogger<CountriesController> logger)
        {
            _countriesRepository = countryRepo;
            _mapper = mapper;
            _logger = logger;
            _logger = logger;
        }

        // GET: api/Countries
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
        {
            var countries = await _countriesRepository.GetAllAsync<GetCountryDto>();
            return Ok(countries);
        }

        // GET: api/Countries/?PageSize=25&StartIndex=0
        [HttpGet]
        public async Task<ActionResult<PageResult<GetCountryDto>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {
            var pagedCountriesResult = await _countriesRepository.GetAllAsync<GetCountryDto>(queryParameters);
            return Ok(pagedCountriesResult);
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            var country = await _countriesRepository.GetDetails(id);
            return Ok(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountry)
        {
            if (id != updateCountry.Id)
            {
                return BadRequest("Invalid record ID");
            }

            //_countriesRepository.Entry(country).State = EntityState.Modified;
            //var country = await _countriesRepository.GetAsync(id);
            //if (country is null)
            //{
            //    throw new NotFoundException(nameof(PutCountry), id);
            //}

            //_mapper.Map(updateCountry, country); //you see variable country up there? it will change the value inside it with updateCountry(Dto)

            try
            {
                await _countriesRepository.UpdateAsync(id, updateCountry);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto country)
        {
            //Country newCountry = new()
            //{
            //    Name = country.Name,
            //    ShortName = country.ShortName,
            //};
            //var newCountry = _mapper.Map<Country>(country);
            await _countriesRepository.AddAsync<CreateCountryDto, GetCountryDto>(country);
            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Const.Roles.Administrator)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            await _countriesRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
