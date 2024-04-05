using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private readonly ApplicationDbContext _context;

        public TimesheetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Timesheet> GetByIdAsync(int id)
        {
            return await _context.Timesheets.FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Add(Timesheet timesheet)
        {
            _context.Add(timesheet);
            return Save();
        }

        public bool Delete(Timesheet timesheet)
        {
            _context.Remove(timesheet);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Timesheet timesheet)
        {
            _context.Update(timesheet);
            return Save();
        }
    }
}
