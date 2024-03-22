using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IPracticumFormsRepository
    {
        bool Save();
        // PracticumForms Interfaces
        Task<IEnumerable<PracticumForms>> GetAllForms();
        Task<PracticumForms> FormsGetByIdAsync(int id);
        Task<PracticumForms> FormsGetIdAsyncNoTracking(int id);
        bool Add(PracticumForms practicumForms);
        bool Update(PracticumForms practicumForms);
        bool Delete(PracticumForms practicumForms);
        // Form A Interfaces
        Task<IEnumerable<FormA>> GetAllFormA();
        Task<FormA> FormAGetByIdAsync(int id);
        Task<FormA> FormAGetIdAsyncNoTracking(int id);
        bool Add(FormA formA);
        bool Update(FormA formA);
        bool Delete(FormA formA);

        // Form B Interfaces

        // Form C Interfaces
        Task<IEnumerable<FormC>> GetAllFormC();
        Task<FormC> FormCGetByIdAsync(int id);
        Task<FormC> FormCGetIdAsyncNoTracking(int id);
        bool Add(FormC formC);
        bool Update(FormC formC);
        bool Delete(FormC formC);

        // Form D Fnterfaces
        Task<IEnumerable<FormD>> GetAllFormD();
        Task<FormD> FormDGetByIdAsync(int id);
        Task<FormD> FormDGetIdAsyncNoTracking(int id);
        bool Add(FormD formD);
        bool Update(FormD formD);
        bool Delete(FormD formD);

        // Form FOIP Interfaces

        Task<IEnumerable<FormFOIP>> GetAllFormFOIP();
        Task<FormFOIP> FormFOIPGetByIdAsync(int id);
        Task<FormFOIP> FormFOIPGetIdAsyncNoTracking(int id);
        bool Add(FormFOIP formFOIP);
        bool Update(FormFOIP formFOIP);
        bool Delete(FormFOIP formFOIP);

        // Form StuInfo Interfaces

        Task<IEnumerable<FormStuInfo>> GetAllFormStuInfo();
        Task<FormStuInfo> FormStuInfoGetByIdAsync(int id);
        Task<FormStuInfo> FormStuInfoGetIdAsyncNoTracking(int id);
        bool Add(FormStuInfo formStuInfo);
        bool Update(FormStuInfo formStuInfo);
        bool Delete(FormStuInfo formStuInfo);
    }
}
