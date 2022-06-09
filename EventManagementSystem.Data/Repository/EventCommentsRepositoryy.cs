using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository
{
    public class EventCommentsRepositoryy : Repository<EventCommentsModel>, IEventCommentsRepository
    {
        private readonly AppDbContext db;

        public EventCommentsRepositoryy(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public new EventCommentsModel GetFirstOrDefault(Expression<Func<EventCommentsModel, bool>> filter)
        {
            IQueryable<EventCommentsModel> query = dbSet;
            query = query.Where(filter);
            query = query
                .Include(emp => emp.EventID)
                .ThenInclude(i=>i.EventRecordHanledBy)
                .Include(emp => emp.CommentedUser);
            return query.FirstOrDefault();
        }

        public List<EventCommentsModel> GetListWhereAndInclude(string ID)
        {
            IQueryable<EventCommentsModel> query = dbSet;
            query = query.Where(i => i.CommentedUser.ID == ID).Include(emp => emp.CommentedUser).Include(i=>i.EventID);
            return query.ToList();
        }

        public IEnumerable<EventCommentsModel> GetListInclude()
        {
            IQueryable<EventCommentsModel> query = dbSet;
            query = query.Include(i => i.EventID)
                .ThenInclude(i => i.EventRecordHanledBy)
                .Include(i=>i.CommentedUser);
            return query.ToList();
        }

        public List<EventCommentsModel> GetPublicEventListAndInclude(Expression<Func<EventCommentsModel, bool>> filter)
        {
            IQueryable<EventCommentsModel> query = dbSet;
            query = query.Where(filter)
                .Include(emp => emp.CommentedUser)
                .Include(i=>i.EventID);
            return query.ToList();
        }

        public void Update(EventCommentsModel Entity)
        {
            db.EventCommentstData.Update(Entity);
        }
        public async Task<int> SaveAll()
        {
            //db.Entry(dbSet).State = EntityState.Modified;

            int Changes = await db.SaveChangesAsync();
            return Changes;
        }
    }
}
