using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Client.ViewModels
{
    public class EmployeeViewModel
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string fName { get; set; }
        [Required]
        public string lName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTimeOffset DoB { get; set; }
        [Required]
        public string NIC { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Contact { get; set; }
        [Required]
        public string Status { get; set; }

        //Role And Password
        [Required]
        public string EmpRole { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string EmpPassWord { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("EmpPassWord", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
    }

    public class EmployeeDetailsViewModel
    {
        public EmployeeViewModel EmployeeVM { get; set; }
        public List<EventViewModel> EventVMList { get; set; }
    }
}
