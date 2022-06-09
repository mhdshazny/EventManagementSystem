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
    [ApiController]
    [Route("api/employee/{EmpID}/empevent")]
    [ApiExplorerSettings(GroupName = "employee")]
    public class EmpEventController : ControllerBase
    {
        private readonly EventService service;
        private readonly LinkGenerator linkGenerator;

        public EmpEventController(AppDbContext Db, LinkGenerator linkGenerator)
        {
            service = new EventService(Db);
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public ActionResult<List<EventModel>> GetAll(string EmpID)
        {
            try
            {
                var Result = service.GetEmpEventList(EmpID);
                return Ok(Result);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Server Error :" + er.Message);
            }
        }
    }
}
