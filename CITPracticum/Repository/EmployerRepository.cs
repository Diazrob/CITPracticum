using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Employer employer)
        {
            _context.Add(employer);
            return Save();
        }

        public bool Delete(Employer employer)
        {
            _context.Remove(employer);
            return Save();
        }

        public async Task<IEnumerable<Employer>> GetAll()
        {
            return await _context.Employers.ToListAsync();
        }

        public async Task<Employer> GetByIdAsync(int id)
        {
            return await _context.Employers.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Employer> GetIdAsyncNoTracking(int id)
        {
            return await _context.Employers.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Employer employer)
        {
            _context.Update(employer);
            return Save();
        }
    }
}
