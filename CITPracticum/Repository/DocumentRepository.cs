using CITPracticum.Data;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using Microsoft.EntityFrameworkCore;

namespace CITPracticum.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Document document)
        {
            _context.Add(document);
            return Save();
        }

        public bool Delete(Document document)
        {
            _context.Remove(document);
            return Save();
        }

        public async Task<IEnumerable<Document>> GetAll()
        {
            return await _context.Documents.ToListAsync();
        }

        public async Task<Document> GetByIdAsync(int id)
        {
            return await _context.Documents.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Document> GetIdAsyncNoTracking(int id)
        {
            return await _context.Documents.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Document document)
        {
            _context.Update(document);
            return Save();
        }
    }
}
