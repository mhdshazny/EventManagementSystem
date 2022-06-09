using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Table("Tbl_Employee")]
    [Index(nameof(Email), IsUnique = true)]
    public class EmployeeModel
    {
        [Key]
        public string ID { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DoB { get; set; }
        public string NIC { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }
        public string Status { get; set; }

        //Role And Password
        public string EmpRole { get; set; }
        public string EmpPassWord { get; set; }


    }


}
