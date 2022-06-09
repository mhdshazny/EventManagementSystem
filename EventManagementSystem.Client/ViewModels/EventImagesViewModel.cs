using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.ViewModels
{
    public class EventImagesViewModel
    {
        public EventViewModel Event { get; set; }
        public string ImageName { get; set; }
        public bool IsImageAvailable { get; set; }

        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }

    }
}
