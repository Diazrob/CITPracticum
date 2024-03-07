using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly ApplicationDbContext _context;

        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(JobPosting jobPosting)
        {
            _context.Add(jobPosting);
            return Save();
        }

        public bool Delete(JobPosting jobPosting)
        {
            _context.Remove(jobPosting);
                return Save();
        }

        public async Task<IEnumerable<JobPosting>> GetAll()
        {
            return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting> GetByIdAsync(int id)
        {
            return await _context.JobPostings.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<JobPosting> GetIdAsyncNoTracking(int id)
        {
            return await _context.JobPostings.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(JobPosting jobPosting)
        {
            _context.Update(jobPosting);
            return Save();
        }
    }
}
