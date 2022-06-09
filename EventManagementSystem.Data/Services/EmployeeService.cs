using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository;
using EventManagementSystem.Repository.IRepository;
using EventManagementSystem.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventManagementSystem.Services
{
    public class EmployeeService : IService<EmployeeModel>
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(AppDbContext Db)
        {
            _repo = new EmployeeRepository(Db);
        }

        public async override Task<bool> Add(EmployeeModel collection)
        {            
            bool SuccessFlag = false;

            try
            {
                if (IsValid(collection))
                {
                    _repo.Add(collection);
                    _repo.Save();
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
                if (Model!=null)
                {
                    _repo.Remove(Model);
                    _repo.Save();
                    success = true;
                }
                return success;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public override EmployeeModel Find(string id)
        {
            try
            {
                EmployeeModel Data = null;
                Data = _repo.GetFirstOrDefault(i=>i.ID==id);

                return Data;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        internal async Task<EmployeeModel> VerifyUser(string username, string password)
        {
            try
            {
                EmployeeModel Data = null;
                Data = await _repo.VerifyUser(username,password);

                return Data;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public override List<EmployeeModel> GetList()
        {
            List<EmployeeModel> EmpList = new List<EmployeeModel>();
            try
            {
                EmpList = _repo.GetList().ToList();
                return EmpList;
            }
            catch (Exception er)
            {
                return EmpList;
            }
        }


        public override bool IsValid(EmployeeModel collection)
        {
            bool Valid = true;
            if (collection.ID == null && collection.ID!=NewID().NewID) Valid = false;
            if (collection.Address == null) Valid = false;
            if (collection.Contact == null) Valid = false;
            if (collection.DoB == null) Valid = false;
            if (collection.Email == null) Valid = false;
            if (collection.EmpPassWord == null) Valid = false;
            if (collection.EmpRole == null) Valid = false;
            if (collection.fName == null) Valid = false;
            if (collection.Gender == null) Valid = false;
            if (collection.lName == null) Valid = false;
            if (collection.NIC == null) Valid = false;
            if (collection.Status == null) Valid = false;

            return Valid;
        }

        public override bool Update(EmployeeModel collection)
        {
            bool UpdateStatus = false;
            try
            {
                if (IsValid(collection))
                {
                    _repo.Update(collection);
                    _repo.Save();
                    UpdateStatus = true;
                }
                return UpdateStatus;
            }
            catch (Exception er)
            {
                return UpdateStatus;
            }
        }

        public EmpTemporaryDataModel NewID()
        {
            try
            {
                EmpTemporaryDataModel Obj = new EmpTemporaryDataModel();
                var NewID = _repo.NewID(i=>i.ID,"EMP");
                Obj.NewID = NewID;
                return Obj;
            }
            catch (Exception er)
            {
                return null;
            }
        }


    }
}
