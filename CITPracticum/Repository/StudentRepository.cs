using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Student student)
        {
            _context.Add(student);
           return Save();
        }

        public bool Delete(Student student)
        {
            _context.Remove(student);
            return Save();
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Student> GetIdAsyncNoTracking(int id)
        {
            return await _context.Students.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Student student)
        {
            _context.Update(student);
            return Save();
        }
    }
}
