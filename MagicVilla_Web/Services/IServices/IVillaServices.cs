using MagicVilla_Web.Models;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaServices
    {
        public Task<T> GetAllAsync<T>();
        public Task<T> GetAsync<T>(int id);
        public Task<T> GetAsync<T>(string name);
        public Task<T> CreateAsync<T>(Villa villa);
        public Task<T> UpdateAsync<T>(Villa villa);
        public Task<T> DeleteAsync<T>(int id);
    }
}
