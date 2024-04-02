using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Address address)
        {
            _context.Add(address);
            return Save();
        }

        public bool Delete(Address address)
        {
            _context.Remove(address);
            return Save();
        }

        public async Task<IEnumerable<Address>> GetAll()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address> GetByIdAsync(int id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Address> GetIdAsyncNoTracking(int id)
        {
            return await _context.Addresses.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Address address)
        {
            _context.Update(address);
            return Save();
        }
    }
}
