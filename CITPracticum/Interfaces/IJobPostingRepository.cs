using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IJobPostingRepository
    {
        Task<IEnumerable<JobPosting>> GetAll();
        Task<JobPosting> GetByIdAsync(int id);
        Task<JobPosting> GetIdAsyncNoTracking(int id);
        bool Add(JobPosting jobPosting);
        bool Update(JobPosting jobPosting);
        bool Delete(JobPosting jobPosting);
        bool Save();
    }
}
