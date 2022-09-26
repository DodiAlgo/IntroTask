using ConsoleApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Repository
{
    public class TextRepository : ITextRepository
    {
        private readonly TextDBEntities _context;

        public TextRepository()
        {
            _context = new TextDBEntities();
        }
        public TextRepository(TextDBEntities context)
        {
            _context = context;
        }
        public IEnumerable<TextTable> GetAll()
        {
            return _context.TextTables.ToList();
        }
        public TextTable GetById(int TextID)
        {
            return _context.TextTables.Find(TextID);
        }
        public void Insert(TextTable textTable)
        {
            _context.TextTables.Add(textTable);
        }
        public void Update(TextTable textTable)
        {
            _context.Entry(textTable).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int TextID)
        {
            TextTable textTable = _context.TextTables.Find(TextID);
            _context.TextTables.Remove(textTable);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
