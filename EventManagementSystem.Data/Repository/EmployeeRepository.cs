using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository
{
    public class EmployeeRepository : Repository<EmployeeModel>, IEmployeeRepository
    {
        private readonly AppDbContext db;

        public EmployeeRepository(AppDbContext db):base(db)
        {
            this.db = db;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(EmployeeModel Entity)
        {
            db.EmpData.Update(Entity);
        }

        public async Task<EmployeeModel> VerifyUser(string username, string password)
        {

            try
            {
                var data = await db.EmpData.Where(i => i.Email == username).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception er)
            {
                return null;
            }
        }
    }
}
