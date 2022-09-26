using ConsoleApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Repository
{
    public interface ITextRepository
    {
        IEnumerable<TextTable> GetAll();
        TextTable GetById(int TextID);
        void Insert(TextTable textTable);
        void Update(TextTable textTable);
        void Delete(int TextID);
        void Save();

    }
}
