using System;
using EventManagementSystem.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using EventManagementSystem.Db;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        public DbSet<T> dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T Entity)
        {
            
            dbSet.Add(Entity);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetList()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public IEnumerable<T> GetListWhere(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.ToList();
        }

        public string NewID(Expression<Func<T, string>> filter, string Prefix)
        {
            var Id = dbSet.Select(filter).Max();
            int num = 1;
            string NewID = Prefix + num.ToString().PadLeft((10 - Prefix.Length), '0');
            if (Id != null)
            {
                string OldToNewID = (int.Parse(Id.Substring(Prefix.Length, (10-Prefix.Length))) + 1).ToString();
                NewID = Prefix + OldToNewID.PadLeft((10-Prefix.Length), '0');
                return NewID;
            }
            return NewID;
        }

        public int NewIntID(Expression<Func<T, int>> filter)
        {
            int Id = dbSet.Select(filter).Max();
            int NewID = 1;
            if (Id != 0)
            {
                NewID = Id + 1;
            }
            return NewID;
        }

        public void Remove(T Entity)
        {
            dbSet.Remove(Entity);
        }
    }
}
