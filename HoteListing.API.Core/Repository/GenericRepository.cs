using HotelListing.API.Core.Interfaces;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.API.Core.Exceptions;

namespace HotelListing.API.Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(HotelListingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity); //it automatically know what table in this entity
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            /*
             * TSource = The DTO to get query(properties)
             * TResult = The DTO to send query(result)
             */
            var entity = _mapper.Map<T>(source);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TResult>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var data = await GetAsync(id);
            if (data is null)
            {
                throw new NotFoundException(typeof(T).Name, id); //can i use nameof instead typeof?
            }
            _context.Set<T>().Remove(data);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var data = await GetAsync(id);
            return data != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters) //TResult usually a DTO
        {
            var totalSize = await _context.Set<T>().CountAsync();
            var items = await _context.Set<T>()
                .Skip(queryParameters.PageNumber > 1 ? (queryParameters.PageSize * (queryParameters.PageNumber - 1)) + queryParameters.StartIndex : queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider) //ProjectTo will make query to database spesificed by generic class, not all property will be fetched
                .ToListAsync();
            return new PageResult<TResult>
            {
                Items = items,
                PageNumber = queryParameters.PageNumber,
                RecordNumber = queryParameters.PageSize,
                TotalCount = totalSize
            };
        }

        public async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _context
                .Set<T>()
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync(); //as soon this syntax executed, query also executed
        }

        public async Task<T> GetAsync(int? Id)
        {
            var result = await _context.Set<T>().FindAsync(Id);
            if (result is null)
            {
                throw new NotFoundException(typeof(T).Name, Id.HasValue ? Id : "Empty"); ;
            }
            return result;
        }

        public async Task<TResult> GetAsync<TResult>(int? Id)
        {
            var result = await _context.Set<T>().FindAsync(Id);
            if (result is null)
            {
                throw new NotFoundException(typeof(T).Name, Id.HasValue ? Id : "Empty"); ;
            }
            return _mapper.Map<TResult>(result);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<TSource>(int id, TSource source)
        {
            var entity = await GetAsync(id);

            if(entity == null)
            {
                throw new NotFoundException(typeof(T).Name, id);
            }
            _mapper.Map(source, entity);
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
