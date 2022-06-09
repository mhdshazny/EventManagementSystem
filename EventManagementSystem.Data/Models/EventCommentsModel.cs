using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Table("Tbl_EventComments")]
    public class EventCommentsModel
    {
        [Required]
        [Key]
        public int ID { get; set; }
        [Required]
        public EventModel EventID { get; set; }
        [Required]
        public EmployeeModel CommentedUser { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
