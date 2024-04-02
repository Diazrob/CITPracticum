using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetAll();
        Task<Address> GetByIdAsync(int id);
        Task<Address> GetIdAsyncNoTracking(int id);
        bool Add(Address address);
        bool Update(Address address);
        bool Delete(Address address);
        bool Save();
    }
}
