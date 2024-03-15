using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IPracticumFormsRepository
    {
        bool Save();
        // form A interfaces
        Task<IEnumerable<FormA>> GetAllFormA();
        Task<FormA> FormAGetByIdAsync(int id);
        Task<FormA> FormAGetIdAsyncNoTracking(int id);
        bool Add(FormA formA);
        bool Update(FormA formA);
        bool Delete(FormA formA);

        // form B interfaces

        // form C interfaces
        Task<IEnumerable<FormC>> GetAllFormC();
        Task<FormC> FormCGetByIdAsync(int id);
        Task<FormC> FormCGetIdAsyncNoTracking(int id);
        bool Add(FormC formC);
        bool Update(FormC formC);
        bool Delete(FormC formC);
        
       
    }
}
