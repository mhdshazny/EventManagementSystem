using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    public class UserModel : IdentityUser
    {
        [DataType(DataType.MultilineText)]
        public string EmpRole { get; set; }
    }
}
