using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository.IRepository
{
    public interface IEventRepository : IRepository<EventModel>
    {
        //IEnumerable<EventModel> GetListInclude(Expression<Func<EventModel>> filte);
        //IEnumerable<EventModel> GetListInclude(string Include);
        List<EventModel> GetListWhereAndInclude(string ID);
        List<EventModel> GetPublicEventListAndInclude(Expression<Func<EventModel,bool>> filter);

        IEnumerable<EventModel> GetListInclude();
        //string GetImageFolder(string ID);

        void Update(EventModel Entity);
        Task<int> Save();
    }
}
