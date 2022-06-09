using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository
{
    public class EventRepository : Repository<EventModel>, IEventRepository
    {
        private readonly AppDbContext db;
        private readonly string _path;

        public EventRepository(AppDbContext db): base(db)
        {
            this.db = db;
            _path = AppDomain.CurrentDomain.BaseDirectory.ToString();
        }

        public async Task<int> Save()
        {
            //db.Entry(dbSet).State = EntityState.Modified;

            int Changes =await db.SaveChangesAsync(); ;
            return Changes;
        }

        public new EventModel GetFirstOrDefault(Expression<Func<EventModel, bool>> filter)
        {
            IQueryable<EventModel> query = dbSet;
            query = query.Where(filter);
            query = query
                .Include(emp => emp.EventRecordHanledBy);

            //foreach (var item in query)
            //{
            //    item.Image = GetImageFolder(item.EventID);
            //}

            return query.FirstOrDefault();
        }
        public List<EventModel> GetListWhereAndInclude(string ID)
        {
            IQueryable<EventModel> query = dbSet;
            query = query.Where(i=>i.EventRecordHanledBy.ID==ID).Include(emp => emp.EventRecordHanledBy); 
            
            //foreach (var item in query)
            //{
            //    item.Image = GetImageFolder(item.EventID);
            //}

            return query.ToList();
        }

        public void Update(EventModel Entity)
        {
            db.EventData.Update(Entity);
        }

        public IEnumerable<EventModel> GetListInclude()
        {
            IQueryable<EventModel> query = dbSet;
            query = query.Include(i=>i.EventRecordHanledBy);
            //foreach (var item in query)
            //{
            //    item.Image = GetImageFolder(item.EventID);
            //}
            return query.ToList();
        }

        public List<EventModel> GetPublicEventListAndInclude(Expression<Func<EventModel,bool>> filter)
        {
            IQueryable<EventModel> query = dbSet;
            query = query.Where(filter).Include(emp => emp.EventRecordHanledBy);
            return query.ToList();
        }

        //public string GetImageFolder(string ID)
        //{
        //    string _diectory = _path+"\\" + ID;

        //    if (FolderExist(_diectory))
        //    {
        //        return _diectory;

        //        //if (FileExist(_diectory,ID))
        //        //{
        //        //    return _diectory+"\\" + ID;
        //        //}
        //        //else
        //        //{
        //        //    return "No Images Found";
        //        //}
        //    }
        //    else
        //    {
        //        CreateFolder(_diectory);
        //        return _diectory;
        //    }
        //}


        //private bool CreateFolder(string diectory)
        //{
        //    var IsExist = Directory.CreateDirectory(diectory);
        //    return IsExist.Exists;
        //}

        //private bool FolderExist(string directory)
        //{
        //    bool IsExist = Directory.Exists(directory);
        //    return IsExist;
        //}

        //private bool FileExist(string directory, string FileName)
        //{
        //    var IsExist = File.Exists(directory+"\\"+FileName);
        //    return IsExist;
        //}

        //public IEnumerable<EventModel> GetListInclude(string Include)
        //{
        //    IQueryable<EventModel> query = dbSet;
        //    query = query.Include(Include);
        //    return query.ToList();
        //}
    }
}
