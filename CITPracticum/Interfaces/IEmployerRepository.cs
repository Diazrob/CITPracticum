using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IEmployerRepository
    {
        Task<IEnumerable<Employer>> GetAll();
        Task<Employer> GetByIdAsync(int id);
        Task<Employer> GetIdAsyncNoTracking(int id);
        bool Add(Employer employer);
        bool Update(Employer employer);
        bool Delete(Employer employer);
        bool Save();
    }
}
