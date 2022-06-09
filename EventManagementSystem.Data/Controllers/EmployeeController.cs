using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService service;
        private readonly LinkGenerator linkGenerator;

        public EmployeeController(AppDbContext Db, LinkGenerator linkGenerator)
        {
            service = new EmployeeService(Db);
            this.linkGenerator = linkGenerator;
        }

        [HttpGet("getAll",Name = "getAll")]
        public ActionResult<List<EmployeeModel>> GetAll()
        {
            List<EmployeeModel> Data = new List<EmployeeModel>();
            try
            {
                Data = service.GetList();
                return Ok(Data);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, Data); ;
            }
        }
        [HttpGet("search")]
        public ActionResult<EmployeeModel> Find(string ID)
        {
            EmployeeModel Data = new EmployeeModel();
            try
            {
                Data = service.Find(ID);
                return Ok(Data);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, Data); ;
            }
        }
        [HttpGet("VerifyUser")]
        public async Task<ActionResult<EmployeeModel>> VerifyUser(string username,string password)
        {
            EmployeeModel Data = new EmployeeModel();
            try
            {
                Data = await service.VerifyUser(username,password);
                return Ok(Data);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, Data); ;
            }
        }
        [HttpPost("add")]
        public async Task<ActionResult<EmployeeModel>> Add(EmployeeModel Model)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("Find", "Employee", new { ID = Model.ID });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Couldn't use current EmployeeID.");
                }

                bool status = await service.Add(Model);
                if (status)
                {
                    return Created(location, Model);
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, Model);
                }
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, Model);
            }
        }

        [HttpPut("upsert")]
        public ActionResult<EmployeeModel> Upsert(EmployeeModel Model)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("Find", "Employee", new { ID = Model.ID });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Couldn't use current EmployeeID.");
                }

                bool status = service.Update(Model);
                if (status)
                {
                    return Ok(Model);
                }
                else
                {
                    return this.StatusCode(StatusCodes.Status500InternalServerError, Model);
                }
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, Model);
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(string ID)
        {
            try
            {
                var Existing = service.Find(ID);

                if (Existing == null) return NotFound("Couldn't Find the relevant Data.");

                bool Deleted = service.Delete(ID);
                if (Deleted)
                {
                    return Ok();
                }

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

            return BadRequest("Deletion Failed");
        }

        [HttpGet("newID")]
        public ActionResult<EmpTemporaryDataModel> NewID()
        {
            try
            {
                var NewIDModel = service.NewID();

                if (NewIDModel.NewID == null) return NotFound("Couldn't generate a new ID.");

                return NewIDModel;

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
