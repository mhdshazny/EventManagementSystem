using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Client.ViewModels
{
    public class EventCommentViewModel
    {
        public EventCommentViewModel()
        {
            Status = "Pending Approval";
        }
        //public int ID { get; set; }
        [Required]
        public EventViewModel EventID { get; set; }
        [Required]
        public EmployeeViewModel CommentedUser { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public string Status { get; set; }
    }

    public class ViewAllCommentViewModel
    {
        public EventViewModel EventID { get; set; }

        public IEnumerable<EventCommentViewModel> CommentsList { get; set; }

    }
}
