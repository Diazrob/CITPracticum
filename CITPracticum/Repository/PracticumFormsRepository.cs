using CITPracticum.Data;
using CITPracticum.Data.Migrations;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class PracticumFormsRepository : IPracticumFormsRepository
    {
        private readonly ApplicationDbContext _context;

        public PracticumFormsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        public bool Add(FormA formA)
        {
            _context.Add(formA);
            return Save();
        }
        public bool Delete(FormA formA)
        {
            _context.Remove(formA);
            return Save();
        }

        public async Task<FormA> FormAGetByIdAsync(int id)
        {
            return await _context.FormAs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormA> FormAGetIdAsyncNoTracking(int id)
        {
            return await _context.FormAs.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormA>> GetAllFormA()
        {
            return await _context.FormAs.ToListAsync();
        }

        public bool Update(FormA formA)
        {
            _context.Update(formA);
            return Save();
        }

        public bool Add(FormC formC)
        {
            _context.Add(formC);
            return Save();
        }
        public bool Delete(FormC formC)
        {
            _context.Remove(formC);
            return Save();
        }

        public async Task<FormC> FormCGetByIdAsync(int id)
        {
            return await _context.FormCs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormC> FormCGetIdAsyncNoTracking(int id)
        {
            return await _context.FormCs.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormC>> GetAllFormC()
        {
            return await _context.FormCs.ToListAsync();
        }

        public bool Update(FormC formC)
        {
            _context.Update(formC);
            return Save();
        }
    }
}
