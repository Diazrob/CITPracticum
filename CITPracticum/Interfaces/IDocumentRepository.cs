using CITPracticum.Models;

namespace CITPracticum.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAll();
        Task<Document> GetByIdAsync(int id);
        Task<Document> GetIdAsyncNoTracking(int id);
        bool Add(Document document);
        bool Update(Document document);
        bool Delete(Document document);
        bool Save();
    }
}
