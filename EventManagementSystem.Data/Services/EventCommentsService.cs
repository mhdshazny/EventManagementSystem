using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository;
using EventManagementSystem.Repository.IRepository;
using EventManagementSystem.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Services
{
    public class EventCommentsService : IService<EventCommentsModel>
    {
        public readonly IEventCommentsRepository repo;
        private readonly AppDbContext db;

        public EventCommentsService(AppDbContext db)
        {
            repo = new EventCommentsRepositoryy(db);
            this.db = db;
        }
        public async override Task<bool> Add(EventCommentsModel collection)
        {
            bool SuccessFlag = false;

            try
            {
                if (IsValid(collection))
                {
                    collection.ID = 0;
                    db.Entry(collection).State = EntityState.Modified;
                    repo.Add(collection);
                    int ChangesCount= await repo.SaveAll();

                    SuccessFlag = true;
                }

                return SuccessFlag;
            }
            catch (Exception er)
            {
                return SuccessFlag;
            }
        }

        public override bool Delete(string id)
        {
            try
            {
                bool success = false;

                var Model = Find(id);
                if (Model != null)
                {
                    repo.Remove(Model);
                    repo.SaveAll();

                    success = true;
                }
                return success;
            }
            catch (Exception er)
            {
                return false;
            }
        }
        public override EventCommentsModel Find(string id)
        {
            try
            {

                EventCommentsModel Data = null;
                Data = repo.GetFirstOrDefault(i=>i.ID==int.Parse(id));

                return Data;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public override List<EventCommentsModel> GetList()
        {
            List<EventCommentsModel> EmpList = new List<EventCommentsModel>();
            try
            {
                EmpList = repo.GetListInclude().ToList();
                return EmpList;
            }
            catch (Exception er)
            {
                return EmpList;
            }
        }

        public override bool IsValid(EventCommentsModel collection)
        {
            bool Valid = true;
            if (collection.ID != 0) Valid = false;
            if (collection.Comment == null) Valid = false;
            if (collection.CommentedUser == null) Valid = false;
            if (collection.EventID == null) Valid = false;
            if (collection.Status == null) Valid = false;

            return Valid;
        }

        public override bool Update(EventCommentsModel collection)
        {
            bool UpdateStatus = false;
            try
            {
                if (IsValid(collection))
                {
                    repo.Update(collection);
                    repo.SaveAll();
                    UpdateStatus = true;
                }
                return UpdateStatus;
            }
            catch (Exception er)
            {
                return UpdateStatus;
            }
        }

        public EventCommentsTemporaryDataModel NewID()
        {
            EventCommentsTemporaryDataModel Obj = new EventCommentsTemporaryDataModel();
            try
            {
                var DataList = GetList();
                Obj.NewID = 1;
                if (DataList.Count>0)
                    Obj.NewID = repo.NewIntID(i => i.ID);

                return Obj;
            }
            catch (Exception er)
            {
                return Obj;
            }
        }
    }
}
