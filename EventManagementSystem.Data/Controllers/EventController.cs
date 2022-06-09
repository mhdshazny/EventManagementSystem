using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository.IRepository;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Hosting;
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
    [ApiExplorerSettings(GroupName = "event")]
    public class EventController : ControllerBase
    {
        private readonly EventService service;
        private readonly LinkGenerator linkGenerator;

        public EventController(AppDbContext db,LinkGenerator linkGenerator)
        {
            service = new EventService(db);
            this.linkGenerator = linkGenerator;
        }


        [HttpGet]
        public ActionResult<List<EventModel>> GetAll()
        {
            List<EventModel> Data = new List<EventModel>();
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
        [HttpGet("GetPublicEvents")]
        public ActionResult<List<EventModel>> GetPublicEvents()
        {
            List<EventModel> Data = new List<EventModel>();
            try
            {
                Data = service.AllPublicEvents();
                return Ok(Data);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, Data); ;
            }
        }

        [HttpGet("search")]
        public ActionResult<EventModel> Find(string ID)
        {
            EventModel Data = new EventModel();
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
        [HttpPost("add")]
        public async Task<ActionResult<EventModel>> Add(EventModel Model)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("Find", "Event", new { ID = Model.EventID });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Couldn't use current EmployeeID.");
                }

                bool status = await service.Add(Model);
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

        [HttpPut("upsert")]
        public ActionResult<EventModel> Upsert(EventModel Model)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("Find", "Employee", new { ID = Model.EventID });
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
        public ActionResult<EventTemporaryDataModel> NewID()
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
