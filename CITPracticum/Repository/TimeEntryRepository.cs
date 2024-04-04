using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        private readonly ApplicationDbContext _context;

        public TimeEntryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(TimeEntry timeEntry)
        {
            _context.Add(timeEntry);
            return Save();
        }

        public bool Delete(TimeEntry timeEntry)
        {
            _context.Remove(timeEntry);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(TimeEntry timeEntry)
        {
            _context.Update(timeEntry);
            return Save();
        }

        public async Task<IEnumerable<TimeEntry>> GetAll()
        {
            return await _context.TimeEntries.ToListAsync();
        }

        public async Task<TimeEntry> GetByIdAsync(int id)
        {
            return await _context.TimeEntries.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
