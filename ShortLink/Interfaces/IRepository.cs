using ShortLink.Models;
using System;
using System.Collections.Generic;

namespace ShortLink.Interfaces
{
    public interface IRepository : IDisposable
    {
        IEnumerable<Url> GetAll();
        Url GetById(int id);
        void Create(Url u);
        void Edit(Url u);
        void Delete(Url u);
        void Save();
    }
}
