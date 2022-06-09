using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventManagementSystem.Services
{
    public class AuthenicationService
    {
        private IConfiguration _config { get; }
        public readonly EmployeeAPIClient _empAPI;

        public AuthenicationService(IConfiguration config)
        {
            _config = config;
            _empAPI = new EmployeeAPIClient(_config["ApiConfigs:EventManagement:Uri"], new HttpClient());
        }

        internal async Task<EmployeeModel> VerifyUser(string UserName, string Password)
        {
            try
            {
                EmployeeModel result = await _empAPI.VerifyUserAsync(UserName, Password);
                if (result != null)
                {
                    return result;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        internal string EncryptPassword(string Password)
        {
            try
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
                return PasswordHash;
            }
            catch (Exception)
            {
                return null;
            }

        }

        internal bool VerifyPassword(string Password, string PasswordHash)
        {
            bool IsVerified = false;
            try
            {

                IsVerified = BCrypt.Net.BCrypt.Verify(Password, PasswordHash);
                return IsVerified;
            }
            catch (Exception)
            {
                return IsVerified;
            }

        }
    }
}
