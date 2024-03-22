using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class PlacementRepository : IPlacementRepository
    {
        private readonly ApplicationDbContext _context;

        public PlacementRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Placement placement)
        {
            _context.Add(placement);
            return Save();
        }

        public bool Delete(Placement placement)
        {
            _context.Remove(placement);
            return Save();
        }

        public async Task<IEnumerable<Placement>> GetAll()
        {
            return await _context.Placements.ToListAsync();
        }

        public async Task<Placement> GetByIdAsync(int id)
        {
            return await _context.Placements.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Placement> GetIdAsyncNoTracking(int id)
        {
            return await _context.Placements.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Placement placement)
        {
            _context.Update(placement);
            return Save();
        }
    }
}
