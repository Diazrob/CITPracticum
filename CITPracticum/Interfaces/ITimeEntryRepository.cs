using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface ITimeEntryRepository
    {
        Task<TimeEntry> GetByIdAsync(int id);
        Task<IEnumerable<TimeEntry>> GetAll();
        bool Add(TimeEntry timeEntry);
        bool Update(TimeEntry timeEntry);
        bool Delete(TimeEntry timeEntry);
        bool Save();
    }
}
