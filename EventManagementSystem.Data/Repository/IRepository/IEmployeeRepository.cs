using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<EmployeeModel>
    {
        void Update(EmployeeModel Entity);
        void Save();
        Task<EmployeeModel> VerifyUser(string username,string password);
    }
}
