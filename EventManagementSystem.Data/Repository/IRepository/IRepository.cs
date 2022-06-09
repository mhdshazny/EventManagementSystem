using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetList();
        IEnumerable<T> GetListWhere(Expression<Func<T, bool>> filter);
        void Add(T Entity);
        //void AddRange(IEnumerable<T> Collection);
        void Remove(T Entity);
        //void RemoveRange(IEnumerable<T> Collection);
        string NewID(Expression<Func<T, string>> filter, string Prefix);
        int NewIntID(Expression<Func<T, int>> filter);
    }
}
