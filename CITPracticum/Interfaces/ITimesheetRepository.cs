using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface ITimesheetRepository
    {
        Task<Timesheet> GetByIdAsync(int id);
        bool Add(Timesheet timesheet);
        bool Update(Timesheet timesheet);
        bool Delete(Timesheet timesheet);
        bool Save();
    }
}
