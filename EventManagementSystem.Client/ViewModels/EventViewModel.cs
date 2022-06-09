using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.ViewModels
{
    public class EventViewModel
    {
        public EventViewModel()
        {
            IsImagesAvailable = true;
        }
        [Required]
        public string EventID { get; set; }
        [Required]

        public string EventTitle { get; set; }
        [Required]

        public DateTimeOffset Date { get; set; }
        [Required]

        public long Duration { get; set; }

        public string Description { get; set; }
        [Required]

        public string Publisher { get; set; }
        [Required]

        public string Location { get; set; }
        [Required]

        public string Status { get; set; }
        [NotMapped]
        //[Required]
        [DataType(DataType.Upload)]
        public List<IFormFile> Images { get; set; }
        [NotMapped]
        //[Required]
        public List<string> ImageNameList { get; set; }
        [NotMapped]
        //[Required]
        public bool IsImagesAvailable { get; set; }


        //[NotMapped]
        ////[Required]
        //[DataType(DataType.Upload)]
        //public List<EventImagesViewModel> ViewImages { get; set; }

        //public string EventRecordHanledBy { get; set; }

        //Record Maintaining
        //[Required]

        public EmployeeViewModel EventRecordHanledBy { get; set; }
        [Required]

        public string Visibility { get; set; }
    }
}
