using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Client.ViewModels
{
    public class ServiceReturnViewModel
    {
        public ServiceReturnViewModel()
        {
            IsSuccess = true;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
