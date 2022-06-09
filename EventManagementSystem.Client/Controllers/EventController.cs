using AutoMapper;
using EventManagementSystem.Authentication;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    public class EventController : Controller
    {
        private string Role;
        public readonly EventImageService _imageService;
        public readonly EventService _service;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public EventController(IConfiguration config, IMapper mapper,IWebHostEnvironment environment, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            _imageService = new EventImageService(config, mapper,environment);
            _service = new EventService(config, mapper,environment);
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            Role = ClaimsStored.GetRole();
        }
        [ActionName("Index")]
        public async Task<IActionResult> Index(EventViewModel Model)
        {
            if (!TempData.ContainsKey("InOut"))
            {
                ViewBag.InOut = "In";
            }
            else
            {
                ViewBag.InOut = TempData["InOut"].ToString();
                if (Model.IsImagesAvailable)
                {
                    if (Model.ImageNameList.Count > 0 || Model.ImageNameList != null)
                    {
                        ViewBag.ImageData = Model.ImageNameList;
                        ViewBag.IsImagesAvailable = true;
                    }
                    ViewBag.EventID = Model.EventID;
                    //ViewBag.EventData = TempData["ModelData"];
                }
                else
                {
                    ViewBag.EventID = Model.EventID;
                    ViewBag.IsImagesAvailable = false; 
                    //ViewBag.EventData = TempData["ModelData"];


                }


                //TempData.Clear();
            }

            var Data = await _service.GetList();
            ViewBag.NewID = await _service.NewID();
            var data1 = User;
            var data2 = Request.Headers["Authorization"];
            return View(Data);
        }


        [HttpPost]
        public async Task<IActionResult> Insert(EventViewModel model)
        {
            try
            {


                if (CheckValidationStatus(model))
                {

                    var Result = await _service.AddAsync(model);
                    if (Result)
                    {
                        //TempData["InOut"] = "In";
                        //ViewBag.ReturnMsg = "Data Successfully Added.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        //TempData["InOut"] = "In";
                        ViewBag.ReturnMsg = "Data Insertion Failed.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    TempData["InOut"] = "In";
                    ViewBag.ReturnMsg = "Invalid Data.";
                    return RedirectToAction(nameof(Index), model);
                }
            }
            catch (Exception er)
            {
                TempData["InOut"] = "In";

                ViewBag.ReturnMsg = "Data Insertion Error.";
                return RedirectToAction(nameof(Index), model);
            }
        }

        private bool CheckValidationStatus(EventViewModel modelState)
        {
            bool status = true;
            if (modelState.EventID == null)     status = false;
            if (modelState.Date == null)        status = false;
            if (modelState.Duration < 1)        status = false;
            if (modelState.EventTitle == null)  status = false;
            if (modelState.Location == null)    status = false;
            if (modelState.Publisher == null)   status = false;
            if (modelState.Visibility == null)  status = false;
            if (modelState.Status == null)      status = false;

            return status;
        }

        public class EditModule
        {
            public EventViewModel Event { get; set; }
            public EmployeeViewModel HandlerEmployee { get; set; }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                TempData["InOut"] = "Up";
                var Data = await _service.Find(id);
                //TempData["ModelData"] = Data.EventRecordHanledBy;
                EditModule editModule = new EditModule
                {
                    Event = Data,
                    HandlerEmployee = Data.EventRecordHanledBy
                };
                return RedirectToAction(nameof(Index), Data);
            }
            catch (Exception err)
            {
                err.ToString();
                //TempData["InOut"] = "In";
                return RedirectToAction(nameof(Index));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(EventViewModel model)
        {
            try
            {
                //ModelState.Remove(nameof(model.ConfirmPassword));
                if (!ModelState.IsValid)
                {
                    var Result = await _service.Update(model);
                    if (Result)
                    {
                        //ViewBag.ReturnMsg = "Data Successfully Updated.";
                        //TempData["InOut"] = "In";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        //ViewBag.ReturnMsg = "Data Updation Failed.";
                        TempData["InOut"] = "Up";
                        return RedirectToAction(nameof(Index),model);
                    }
                }
                else
                {
                    //return RedirectToAction(nameof(Index),"Employee", model, "Invalid Data");
                    //ViewBag.ReturnMsg = "Data Updation Error.";
                    TempData["InOut"] = "Up";
                    return RedirectToAction(nameof(Index), model);
                }
            }
            catch (Exception er)
            {
                //ViewBag.ReturnMsg = "Data Updation Error.";
                TempData["InOut"] = "Up";
                return RedirectToAction(nameof(Index), model);
            }
        }


        public async Task<IActionResult> Details(string id)
        {
            try
            {
                EventViewModel Data = await _service.FindDetails(id);
                return View(Data);
            }
            catch (Exception er)
            {
                TempData["InOut"] = "In";
                er.ToString();
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpPost]
        public async Task<string> Delete(string id)
        {
            try
            {
                if (id != null)
                {


                    bool AddData = await _service.Delete(id);

                    if (AddData)
                    {
                        return "Success";
                    }
                    else
                        return "Failed";

                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception err)
            {
                TempData["InOut"] = "In";
                return err.Message;
            }
        }

        [HttpPost]
        public async Task<string> DeleteImage(string id,string EventID)
        {
            try
            {
                if (!string.IsNullOrEmpty(id)&&!string.IsNullOrEmpty(EventID))
                {


                    bool AddData = await _imageService.DeleteImage(id,EventID);

                    if (AddData)
                    {
                        return "Success";
                    }
                    else
                        return "Failed";

                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception err)
            {
                TempData["InOut"] = "In";
                return err.Message;
            }
        }
        [HttpPost]
        [Authorize(Roles ="Employee")]
        public async Task<string> CloseEvent(string id)
        {
            try
            {
                if (id != null)
                {


                    bool AddData = await _service.CloseEvent(id);

                    if (AddData)
                    {
                        return "Success";
                    }
                    else
                        return "Failed";

                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception err)
            {
                TempData["InOut"] = "In";
                return err.Message;
            }
        }
    }
}
