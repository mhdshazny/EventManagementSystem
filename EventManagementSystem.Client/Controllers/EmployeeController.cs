using AutoMapper;
using EventManagementSystem.Client.Services;
using EventManagementSystem.Client.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventManagementSystem.Client.Controllers
{
    public class EmployeeController : Controller
    {
        public readonly EmployeeService _service;

        public EmployeeController(IConfiguration config, IMapper mapper)
        {
            _service = new EmployeeService(config,mapper);
        }
        public async Task<IActionResult> Index(EmployeeViewModel Model)
        {
            if (!TempData.ContainsKey("InOut"))
            {
                ViewBag.InOut = "In";
            }
            else
            {
                ViewBag.InOut = TempData["InOut"].ToString();
                TempData.Clear();
            }

            var Data = await _service.GetList();
            ViewBag.NewID = await _service.NewID();
            return View(Data);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(EmployeeViewModel model)
        {
            try
            {
                
                if (ModelState.IsValid)
                {

                    var Result = await _service.AddAsync(model);
                    if (Result)
                    {
                        ViewBag.InOut = "In";
                        //ViewBag.ReturnMsg = "Data Successfully Added.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewBag.InOut = "In";
                        ViewBag.ReturnMsg = "Data Insertion Failed.";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ViewBag.ReturnMsg = "Invalid Data.";
                    //return RedirectToAction(nameof(Index),"Employee", model, "Invalid Data");
                    return RedirectToAction(nameof(Index),model);
                }
            }
            catch (Exception er)
            {
                ViewBag.ReturnMsg = "Data Insertion Error.";
                return RedirectToAction(nameof(Index), model);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                TempData["InOut"] = "Up";
                var Data = await _service.Find(id);
                return RedirectToAction(nameof(Index), Data);
            }
            catch (Exception err)
            {
                err.ToString();
                TempData["InOut"] = "In";
                return RedirectToAction(nameof(Index));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(EmployeeViewModel model)
        {
            try
            {
                ModelState.Remove(nameof(model.ConfirmPassword));
                if (ModelState.IsValid)
                {
                    var Result = await _service.Update(model);
                    if (Result)
                    {
                        //ViewBag.ReturnMsg = "Data Successfully Updated.";
                        ViewBag.InOut = "Up";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        //ViewBag.ReturnMsg = "Data Updation Failed.";
                        ViewBag.InOut = "Up";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    //return RedirectToAction(nameof(Index),"Employee", model, "Invalid Data");
                    //ViewBag.ReturnMsg = "Data Updation Error.";
                    ViewBag.InOut = "Up";
                    return RedirectToAction(nameof(Index),model);
                }
            }
            catch (Exception er)
            {
                //ViewBag.ReturnMsg = "Data Updation Error.";
                ViewBag.InOut = "Up";
                return RedirectToAction(nameof(Index), model);
            }
        }


        public async Task<IActionResult> Details(string id)
        {
            try
            {
                EmployeeDetailsViewModel Data = await _service.FindDetails(id);
                return View(Data);
            }
            catch (Exception er)
            {
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
                return err.Message;
            }
        }
    }
}
