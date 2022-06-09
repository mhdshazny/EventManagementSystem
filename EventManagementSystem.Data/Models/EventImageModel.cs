using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Models
{
    [Table("tbl_EventImages")]
    public class EventImageModel
    {
        [Key]
        public int ID { get; set; }
        public EventModel Event { get; set; }
        public string ImageName { get; set; }
        public bool IsImageAvailable { get; set; }
    }
}
