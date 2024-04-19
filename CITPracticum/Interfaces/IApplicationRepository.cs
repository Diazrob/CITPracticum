using CITPracticum.Models;
using System.Linq.Expressions;

namespace CITPracticum.Interfaces
{
    public interface IApplicationRepository
    {
        Task<IEnumerable<Application>> GetAll();
        Task<Application> GetByIdAsync(int id);
        bool Add(Application application);
        bool Update(Application application);
        bool Delete(Application application);
        Task<bool> AnyAsync(Expression<Func<Application, bool>> predicate);
        bool Save();
    }
}
