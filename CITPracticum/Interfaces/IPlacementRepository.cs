using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IPlacementRepository
    {
        Task<IEnumerable<Placement>> GetAll();
        Task<Placement> GetByIdAsync(int id);
        Task<Placement> GetIdAsyncNoTracking(int id);
        bool Add(Placement placement);
        bool Update(Placement placement);
        bool Delete(Placement placement);
        bool Save();

        bool Add(Employer employer);
    }
}
