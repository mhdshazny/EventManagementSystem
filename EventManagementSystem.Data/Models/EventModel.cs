using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Table("tbl_Events")]
    public class EventModel
    {
        [Key]
        public string EventID { get; set; }
        public string EventTitle { get; set; }
        public DateTime Date { get; set; }
        public long Duration { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }

        //[NotMapped]
        //public string Image { get; set; }

        //Record Maintaining
        public EmployeeModel EventRecordHanledBy { get; set; }
        public string Visibility { get; set; }

    }
}
