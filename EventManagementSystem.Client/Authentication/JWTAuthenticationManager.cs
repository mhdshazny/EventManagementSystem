using AutoMapper;
using EventManagementSystem;
using EventManagementSystem;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementSystem.Authentication
{
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly string key;
        private readonly AuthenicationService authenicationService;

        private IConfiguration Config { get; }

        public JWTAuthenticationManager(string key, IConfiguration config)
        {
            this.key = key;
            Config = config;
            authenicationService = new AuthenicationService(config);
        }

        private string GenerateToken(EmployeeModel _user)
        {
            var secuirityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var credentials = new SigningCredentials(secuirityKey,SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,_user.Email),
                new Claim(ClaimTypes.Name,_user.FName+" "+_user.LName),
                new Claim(ClaimTypes.Email,_user.Email),
                new Claim(ClaimTypes.GivenName,_user.FName),
                new Claim(ClaimTypes.Email,_user.Email),
                new Claim(ClaimTypes.Role,_user.EmpRole)

            };

            var Token = new JwtSecurityToken(Config["Jwt:Issuer"],
                Config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials:credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<string> Authenticate(string username, string password)
        {
            string PassHash = authenicationService.EncryptPassword(password);
            var IsVerifiedModel = await authenicationService.VerifyUser(username, PassHash);



            if (IsVerifiedModel == null)
            {
                return null;
            }
            bool PasswordMatched = authenicationService.VerifyPassword(password, IsVerifiedModel.EmpPassWord);
            if (!PasswordMatched)
            {
                return null;
            }

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenKey = Encoding.ASCII.GetBytes(key);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, IsVerifiedModel.Email),
            //        new Claim(ClaimTypes.NameIdentifier, IsVerifiedModel.Id),
            //        new Claim(ClaimTypes.Role,IsVerifiedModel.EmpRole)
            //    }),

            //    Expires = DateTime.UtcNow.AddHours(1),
            //    SigningCredentials =
            //    new SigningCredentials(
            //        new SymmetricSecurityKey(tokenKey),
            //        SecurityAlgorithms.HmacSha256Signature),



            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);


            return GenerateToken(IsVerifiedModel);
        }
    }
}
