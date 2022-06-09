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
    [ApiExplorerSettings(GroupName = "event")]
    public class EventCommentsController : Controller
    {
        private readonly EventCommentsService service;
        private readonly LinkGenerator linkGenerator;

        public EventCommentsController(AppDbContext db, LinkGenerator linkGenerator)
        {
            service = new EventCommentsService(db);
            this.linkGenerator = linkGenerator;
        }
        [HttpGet]
        public ActionResult<List<EventCommentsModel>> GetAll()
        {
            List<EventCommentsModel> Data = new List<EventCommentsModel>();
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
        public ActionResult<EventCommentsModel> Find(int ID)
        {
            EventCommentsModel Data = new EventCommentsModel();
            try
            {
                Data = service.Find(ID.ToString());
                return Ok(Data);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, Data); ;
            }
        }
        [HttpPost("add")]
        public async Task<ActionResult<EventCommentsModel>> Add(EventCommentsModel Model)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("Find", "EventComments", new { ID = Model.ID });
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Couldn't use current EmployeeID.");
                }

                bool status =await service.Add(Model);
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
        public ActionResult<EventCommentsModel> Upsert(EventCommentsModel Model)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("Find", "EventComments", new { ID = Model.ID });
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
        public IActionResult Delete(int ID)
        {
            try
            {
                var Existing = service.Find(ID.ToString());

                if (Existing == null) return NotFound("Couldn't Find the relevant Data.");

                bool Deleted = service.Delete(ID.ToString());
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
        public ActionResult<EventCommentsTemporaryDataModel> NewID()
        {
            try
            {
                var NewIDModel = service.NewID();

                if (NewIDModel == null) return NotFound("Couldn't generate a new ID.");

                return NewIDModel;

            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
    }
}
