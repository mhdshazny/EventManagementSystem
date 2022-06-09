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
    public class EventService : IService<EventModel>
    {
        public readonly IEventRepository repo;
        private readonly AppDbContext db;

        public EventService(AppDbContext db)
        {
            repo = new EventRepository(db);
            this.db = db;
        }
        public async override Task<bool> Add(EventModel collection)
        {
            bool SuccessFlag = false;

            try
            {
                if (IsValid(collection))
                {
                    db.Entry(collection).State = EntityState.Modified;
                    repo.Add(collection);
                    await repo.Save();

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
                    repo.Save();

                    success = true;
                }
                return success;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        internal List<EventModel> AllPublicEvents()
        {
            List<EventModel> Data = new List<EventModel>();
            try
            {
                Data = repo.GetPublicEventListAndInclude(i=>i.Visibility=="Public");
                return Data;
            }
            catch (Exception er)
            {
                return Data;
            }
        }

        public override EventModel Find(string id)
        {
            try
            {
                EventModel Data = null;
                Data = repo.GetFirstOrDefault(i => i.EventID == id);

                return Data;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public override List<EventModel> GetList()
        {
            List<EventModel> EmpList = new List<EventModel>();
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


        public override bool IsValid(EventModel collection)
        {
            bool Valid = true;
            if (collection.EventID == null && collection.EventID != NewID().NewID) Valid = false;
            if (collection.Date == null) Valid = false;
            if (collection.Description == null) Valid = false;
            if (collection.Duration == 0) Valid = false;
            if (collection.EventID == null) Valid = false;
            if (collection.EventRecordHanledBy == null) Valid = false;
            if (collection.EventTitle == null) Valid = false;
            if (collection.Location == null) Valid = false;
            if (collection.Publisher == null) Valid = false;
            if (collection.Status == null) Valid = false;
            if (collection.Visibility == null) Valid = false;

            return Valid;
        }

        public override bool Update(EventModel collection)
        {
            bool UpdateStatus = false;
            try
            {
                if (IsValid(collection))
                {
                    repo.Update(collection);
                    repo.Save();
                    UpdateStatus = true;
                }
                return UpdateStatus;
            }
            catch (Exception er)
            {
                return UpdateStatus;
            }
        }

        public EventTemporaryDataModel NewID()
        {
            EventTemporaryDataModel Obj = new EventTemporaryDataModel();
            try
            {
                Obj.NewID = repo.NewID(i => i.EventID, "EVNT");
                return Obj;
            }
            catch (Exception er)
            {
                return Obj;
            }
        }
        public List<EventModel> GetEmpEventList(string EmpID)
        {
            List<EventModel> EmpList = new List<EventModel>();
            try
            {
                EmpList = repo.GetListWhereAndInclude(EmpID);
                return EmpList;
            }
            catch (Exception er)
            {
                return EmpList;
            }
        }

    }
}
