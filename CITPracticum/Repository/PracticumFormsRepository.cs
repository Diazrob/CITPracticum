using CITPracticum.Data;
using CITPracticum.Data.Migrations;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class PracticumFormsRepository : IPracticumFormsRepository
    {
        private readonly ApplicationDbContext _context;

        public PracticumFormsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
        // Practicum Forms Functions
        public bool Add(PracticumForms practicumForms)
        {
            _context.Add(practicumForms);
            return Save();
        }
        public bool Delete(PracticumForms practicumForms)
        {
            _context.Remove(practicumForms);
            return Save();
        }

        public async Task<PracticumForms> FormsGetByIdAsync(int id)
        {
            return await _context.PracticumForms.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<PracticumForms> FormsGetIdAsyncNoTracking(int id)
        {
            return await _context.PracticumForms.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<PracticumForms>> GetAllForms()
        {
            return await _context.PracticumForms.ToListAsync();
        }

        public bool Update(PracticumForms practicumForms)
        {
            _context.Update(practicumForms);
            return Save();
        }
        // Form A Functions
        public bool Add(FormA formA)
        {
            _context.Add(formA);
            return Save();
        }
        public bool Delete(FormA formA)
        {
            _context.Remove(formA);
            return Save();
        }

        public async Task<FormA> FormAGetByIdAsync(int id)
        {
            return await _context.FormAs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormA> FormAGetIdAsyncNoTracking(int id)
        {
            return await _context.FormAs.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormA>> GetAllFormA()
        {
            return await _context.FormAs.ToListAsync();
        }

        public bool Update(FormA formA)
        {
            _context.Update(formA);
            return Save();
        }
        // Form B Functions
        public bool Add(FormB formB)
        {
            _context.Add(formB);
            return Save();
        }
        public bool Delete(FormB formB)
        {
            _context.Remove(formB);
            return Save();
        }

        public async Task<FormB> FormBGetByIdAsync(int id)
        {
            return await _context.FormBs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormB> FormBGetIdAsyncNoTracking(int id)
        {
            return await _context.FormBs.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormB>> GetAllFormB()
        {
            return await _context.FormBs.ToListAsync();
        }

        public bool Update(FormB formB)
        {
            _context.Update(formB);
            return Save();
        }

        // Form C Functions
        public bool Add(FormC formC)
        {
            _context.Add(formC);
            return Save();
        }
        public bool Delete(FormC formC)
        {
            _context.Remove(formC);
            return Save();
        }

        public async Task<FormC> FormCGetByIdAsync(int id)
        {
            return await _context.FormCs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormC> FormCGetIdAsyncNoTracking(int id)
        {
            return await _context.FormCs.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormC>> GetAllFormC()
        {
            return await _context.FormCs.ToListAsync();
        }

        public bool Update(FormC formC)
        {
            _context.Update(formC);
            return Save();
        }

        // Form D Functions
        public bool Add(FormD formD)
        {
            _context.Add(formD);
            return Save();
        }
        public bool Delete(FormD formD)
        {
            _context.Remove(formD);
            return Save();
        }

        public async Task<FormD> FormDGetByIdAsync(int id)
        {
            return await _context.FormDs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormD> FormDGetIdAsyncNoTracking(int id)
        {
            return await _context.FormDs.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormD>> GetAllFormD()
        {
            return await _context.FormDs.ToListAsync();
        }

        public bool Update(FormD formD)
        {
            _context.Update(formD);
            return Save();
        }

        // Form FOIP Functions
        public bool Add(FormFOIP formFOIP)
        {
            _context.Add(formFOIP);
            return Save();
        }
        public bool Delete(FormFOIP formFOIP)
        {
            _context.Remove(formFOIP);
            return Save();
        }

        public async Task<FormFOIP> FormFOIPGetByIdAsync(int id)
        {
            return await _context.FormFOIPs.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormFOIP> FormFOIPGetIdAsyncNoTracking(int id)
        {
            return await _context.FormFOIPs.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormFOIP>> GetAllFormFOIP()
        {
            return await _context.FormFOIPs.ToListAsync();
        }

        public bool Update(FormFOIP formFOIP)
        {
            _context.Update(formFOIP);
            return Save();
        }

        // Form StuInfo Functions
        public bool Add(FormStuInfo formStuInfo)
        {
            _context.Add(formStuInfo);
            return Save();
        }
        public bool Delete(FormStuInfo formStuInfo)
        {
            _context.Remove(formStuInfo);
            return Save();
        }

        public async Task<FormStuInfo> FormStuInfoGetByIdAsync(int id)
        {
            return await _context.FormStuInfos.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FormStuInfo> FormStuInfoGetIdAsyncNoTracking(int id)
        {
            return await _context.FormStuInfos.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<FormStuInfo>> GetAllFormStuInfo()
        {
            return await _context.FormStuInfos.ToListAsync();
        }

        public bool Update(FormStuInfo formStuInfo)
        {
            _context.Update(formStuInfo);
            return Save();
        }
    }
}
