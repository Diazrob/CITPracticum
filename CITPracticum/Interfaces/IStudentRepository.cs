using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAll();
        Task<Student> GetByIdAsync(int id);
        Task<Student> GetIdAsyncNoTracking(int id);
        bool Add(Student student);
        bool Update(Student student);
        bool Delete(Student student);
        bool Save();
    }
}
