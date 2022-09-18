using HotelListing.API.Data;

namespace HotelListing.API.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? Id);
        Task<TResult> GetAsync<TResult>(int? Id);
        Task<List<T>> GetAllAsync();
        Task<List<TResult>> GetAllAsync<TResult>();
        Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
        Task<T> AddAsync(T entity);
        Task<TResult> AddAsync<TSource, TResult>(TSource source);
        Task UpdateAsync(T entity);
        Task UpdateAsync<TSource>(int id,TSource source);
        Task DeleteAsync(int id);
        Task<bool> Exists(int id);
    }
}
