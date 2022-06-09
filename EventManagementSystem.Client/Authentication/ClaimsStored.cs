using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Client.Authentication
{
    public static class ClaimsStored
    {
        public static string Role;
        static ClaimsStored()
        {

        }

        public static void SetRole(string role)
        {
            Role = role;
        }
        public static string GetRole()
        {
            return Role;
        }
    }
}
