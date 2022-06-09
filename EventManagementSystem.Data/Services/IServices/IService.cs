using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Services.IServices
{
    public abstract class IService<T> where T:class
    {
        public abstract List<T> GetList();
        //public abstract List<T> GetListWhere(Expression<Func<T, bool>> filter);
        public abstract Task<bool> Add(T collection);
        public abstract bool Update(T collection);
        public abstract bool IsValid(T collection);
        public abstract bool Delete(string id);
        public abstract T Find(string id);
        //public abstract  NewID();

        public string GetNewID(int OldID, string Prefix)
        {
            OldID = OldID + 1;
            int Length = 10 - Prefix.Length;
            string NewID = Prefix + OldID.ToString().PadLeft(Length, '0');
            return NewID;
        }
    }
}
