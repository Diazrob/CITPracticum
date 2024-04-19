using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CITPracticum.Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Application application)
        {
            _context.Add(application);
            return Save();
        }

        public bool Delete(Application application)
        {
            _context.Remove(application);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Application application)
        {
            _context.Update(application);
            return Save();
        }

        public async Task<IEnumerable<Application>> GetAll()
        {
            return await _context.Applications.ToListAsync();
        }

        public async Task<Application> GetByIdAsync(int id)
        {
            return await _context.Applications.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> AnyAsync(Expression<Func<Application, bool>> predicate)
        {
            return await _context.Applications.AnyAsync(predicate);
        }

    }
}
