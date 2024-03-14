using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IPracticumFormsRepository
    {
        Task<IEnumerable<FormA>> GetAllFormA();
        Task<FormA> FormAGetByIdAsync(int id);
        Task<FormA> FormAGetIdAsyncNoTracking(int id);
        bool Add(FormA formA);
        bool Update(FormA formA);
        bool Delete(FormA formA);
        bool Save();
    }
}
