using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EventManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "event")]
    public class EventImageController : ControllerBase
    {
        private readonly EventService _eventservice;
        private readonly EventImageService _imageservice;
        public EventImageController(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _imageservice = new EventImageService(dbContext,environment);
            _eventservice = new EventService(dbContext);
        }


        // GET: api/<EventImageController>
        [HttpGet]
        public ActionResult<IEnumerable<EventImageModel>> Get(string EventID)
        {
            IEnumerable<EventImageModel> eventImageModel = new List<EventImageModel>();
            try
            {
                eventImageModel = _imageservice.FindImages(EventID);

                return Ok(eventImageModel);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, eventImageModel);
            }
        }


        // POST api/<EventImageController>
        [HttpPost("AddImage")]
        public async Task<ActionResult<bool>> AddImage(EventImageModel eventImage)
        {
            bool IsSuccess = false;

            try
            {
                IsSuccess = await _imageservice.Add(eventImage);
                return Ok(IsSuccess);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, IsSuccess);
            }
        }

        // POST api/<EventImageController>
        [HttpPost("AddImageRange")]
        public async Task<ActionResult<bool>> AddImageRange(List<EventImageModel> eventImageList)
        {
            bool IsSuccess = false;

            try
            {
                IsSuccess = await _imageservice.AddImageRange(eventImageList);
                return Ok(IsSuccess);
            }
            catch (Exception er)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, IsSuccess);
            }
        }

        [HttpDelete("deleteimage")]
        public async Task<ActionResult<bool>> DeleteImage(string ID, string EventID)
        {
            try
            {
                var Existing = _eventservice.Find(EventID);

                if (Existing == null) return NotFound("Couldn't Find the relevant Data.");

                var ExistingImages = _imageservice.FindImages(Existing.EventID);

                if (ExistingImages.Count <= 0) return NotFound("Couldn't Find the relevant Data.");

                bool Deleted = await _imageservice.DeleteImages(ID, Existing, ExistingImages);
                if (Deleted)
                {
                    return Ok(true);
                }
                else
                {
                    //return this.StatusCode(StatusCodes.Status500InternalServerError);
                    return NotFound(false);
                }

            }
            catch (Exception er)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError);
                return NotFound(false);
            }

        }

    }
}
