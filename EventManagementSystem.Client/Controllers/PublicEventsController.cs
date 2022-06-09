using AutoMapper;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    public class PublicEventsController : Controller
    {
        public readonly EventService _service;

        public PublicEventsController(IConfiguration config, IMapper mapper, IWebHostEnvironment environment)
        {
            _service = new EventService(config, mapper,environment);
        }

        public async Task<IActionResult> Index()
        {
            List<EventViewModel> VM = new List<EventViewModel>();
            try
            {
                VM = await _service.GetAllPublicEventsAsync();
                return View(VM);
            }
            catch (Exception er)
            {
                return View(VM);
            }
        }

        public async Task<IActionResult> AddComment(string EventId)
        {
            EventCommentViewModel VM = new EventCommentViewModel();
            string EmpID = "EMP0000001";
            try
            {
                VM = await _service.ReadyCommentVM(EventId,EmpID);
                return View(VM);
            }
            catch (Exception er)
            {
                return View(VM);
            }

        }

        public async Task<IActionResult> ViewComments(string EventId)
        {
            ViewAllCommentViewModel VM = new ViewAllCommentViewModel();
            //string EmpID = "EMP0000001";
            try
            {
                VM = await _service.GetCommentsVM(EventId);
                return View(VM);
            }
            catch (Exception er)
            {
                return View(VM);
            }

        }


        [HttpPost]
        public async Task<IActionResult> Insert(EventCommentViewModel model)
        {
            try
            {
                string EmpID = "EMP0000001";
                EventCommentViewModel VM = await _service.ReadyCommentVM(model.EventID.EventID,EmpID);
                model.EventID = VM.EventID;
                model.CommentedUser = VM.CommentedUser;

                ModelState.Remove(nameof(model.EventID));
                ModelState.Remove(nameof(model.CommentedUser));

                if (!ModelState.IsValid)
                {

                    bool Result = await _service.AddCommentAsync(model);
                    if (Result)
                    {
                        //TempData["InOut"] = "In";
                        //ViewBag.ReturnMsg = "Data Successfully Added.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        //TempData["InOut"] = "In";
                        //ViewBag.ReturnMsg = "Comment Upload Failed.";
                        return RedirectToAction(nameof(AddComment),model.EventID);
                    }
                }
                else
                {
                    //TempData["InOut"] = "In";
                    //ViewBag.ReturnMsg = "Invalid Data.";
                    return RedirectToAction(nameof(AddComment), model.EventID);
                }
            }
            catch (Exception er)
            {
                //TempData["InOut"] = "In";

                //ViewBag.ReturnMsg = "Comment Upload Error.";
                return RedirectToAction(nameof(AddComment), model.EventID);
            }
        }

    }
}
