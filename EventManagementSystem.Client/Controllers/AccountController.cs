using AutoMapper;
using EventManagementSystem.Authentication;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private EmployeeService _service;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;


        public AccountController(IConfiguration config, IMapper mapper,IJWTAuthenticationManager jWTAuthenticationManager)
        {
            _service = new EmployeeService(config, mapper);
            this.jWTAuthenticationManager = jWTAuthenticationManager;

        }

        public IActionResult Login()
        {
            if (TempData["message"] != null && TempData["ToastrType"] != null)
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.ToastrType = TempData["ToastrType"].ToString();
                TempData.Remove("message");
                TempData.Remove("ToastrType");
            }
            return View();
        }
        public IActionResult Logout()
        {
            //DestroySessions();
            TempData["message"] = "Session Cleared and Returned to Login";
            TempData["ToastrType"] = "error";
            return RedirectToAction(nameof(Login), "Account", "SessionCleared-Successful");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            //DestroySessions();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {

                try
                {
                    var token = await jWTAuthenticationManager.Authenticate(username, password);
                    if (token == null)
                        return Unauthorized();
                    
                    return RedirectToAction("Index", "PublicEvents");
                }
                catch (Exception er)
                {
                    TempData["message"] = "Session Cleared and Returned to Login (Error : LoginFailed-UserCredentialsError)";
                    TempData["ToastrType"] = "error";
                    return RedirectToAction(nameof(Login), "Account", "LoginFailed-UserCredentialsNull");
                }

                //var Verify = service.verify(username, password);
                //if (Verify.Status)
                //{
                //    InitalizeSessions(Verify);
                //    TempData["message"] = Verify.Role + " Access Granted";
                //    TempData["ToastrType"] = "success";
                //return RedirectToAction(nameof(Index), "Home", "LoginSuccess");
                //}
                //else
                //{
                //    TempData["message"] = "Session Cleared and Returned to Login (Error : LoginFailed-UserCredentialsError)";
                //    TempData["ToastrType"] = "error";
                //    return RedirectToAction(nameof(Login), "Identity", "LoginFailed-UserCredentialsError");
                //}
            }
            else
            {
                TempData["message"] = "Session Cleared and Returned to Login (Error : LoginFailed-UserCredentialsError)";
                TempData["ToastrType"] = "error";
                return RedirectToAction(nameof(Login), "Account", "LoginFailed-UserCredentialsNull");
            }
        }


        //public void InitalizeSessions(AccountService.SessionData type)
        //{
        //    HttpContext.Session.SetString("UserLogin", "true");
        //    HttpContext.Session.SetString("UserType", type.Role);
        //    HttpContext.Session.SetString("UserID", type.ID);
        //    HttpContext.Session.SetString("UserEmail", type.Email);
        //}
        //public void DestroySessions()
        //{
        //    HttpContext.Session.SetString("UserLogin", "false");
        //    HttpContext.Session.SetString("UserType", "");
        //    HttpContext.Session.SetString("UserID", "");
        //    HttpContext.Session.SetString("UserHotelID", "");
        //    HttpContext.Session.SetString("UserEmail", "");
        //}
    }
}
