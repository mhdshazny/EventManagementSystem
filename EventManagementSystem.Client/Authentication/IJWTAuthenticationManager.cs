using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Authentication
{
    public interface IJWTAuthenticationManager
    {
        Task<string> Authenticate(string username, string password);

    }
}
