using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository.IRepository
{
    public interface IEventCommentsRepository : IRepository<EventCommentsModel>
    {
        List<EventCommentsModel> GetListWhereAndInclude(string ID);
        List<EventCommentsModel> GetPublicEventListAndInclude(Expression<Func<EventCommentsModel, bool>> filter);

        IEnumerable<EventCommentsModel> GetListInclude();

        void Update(EventCommentsModel Entity); 
        Task<int> SaveAll();
    }
}
