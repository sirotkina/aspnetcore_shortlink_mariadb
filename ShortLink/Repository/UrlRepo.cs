using Microsoft.EntityFrameworkCore;
using ShortLink.Interfaces;
using ShortLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortLink.Repository
{
    public class UrlRepo : IRepository
    {
        ShortLinkDbContext db;

        public UrlRepo(ShortLinkDbContext db)
        {
            this.db = db;
        }

        public void Create(Url u)
        {
            db.Entry(u).State = EntityState.Added;
        }

        public void Delete(Url u)
        {
            db.Entry(u).State = EntityState.Deleted;
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Edit(Url u)
        {
            db.Entry(u).State = EntityState.Modified;
        }

        public IEnumerable<Url> GetAll()
        {
            return db.Urls.AsNoTracking();
        }

        public Url GetById(int id)
        {
            return db.Urls.FirstOrDefault(u => u.Id == id);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}
