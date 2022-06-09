using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Client.Authentication
{
    public interface IJWTAuthenticationManager
    {
        Task<string> Authenticate(string username, string password);

    }
}
