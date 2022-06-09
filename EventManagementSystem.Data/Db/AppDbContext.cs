using EventManagementSystem.Models;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<EmployeeModel> EmpData { get; set; }
        public DbSet<EventModel> EventData { get; set; }
        //public DbSet<UserModel> IdentitytData { get; set; }
        public DbSet<EventCommentsModel> EventCommentstData { get; set; }
        public DbSet<EventImageModel> EventImagetData { get; set; }
    }
}
