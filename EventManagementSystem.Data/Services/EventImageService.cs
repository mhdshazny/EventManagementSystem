using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository;
using EventManagementSystem.Repository.IRepository;
using EventManagementSystem.Services.IServices;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Services
{
    public class EventImageService : IService<EventImageModel>
    {
        private readonly IImageRepository _repo;

        public EventImageService(AppDbContext dbContext, IWebHostEnvironment environment)
        {
            _repo = new ImageRepository(dbContext, environment);
        }
        public async override Task<bool> Add(EventImageModel Data)
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = await _repo.AddImage(Data);
                return IsSuccess;
            }
            catch (Exception)
            {
                return IsSuccess;
            }
        }

        public override bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override EventImageModel Find(string id)
        {
            throw new NotImplementedException();
        }
        public List<EventImageModel> FindImages(string eventID)
        {
            List<EventImageModel> ReturnData = new List<EventImageModel>();
            try
            {
                ReturnData = _repo.eventImages(eventID).ToList();
                return ReturnData;
            }
            catch (Exception er)
            {
                return ReturnData;
            }
        }

        public override List<EventImageModel> GetList()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(EventImageModel collection)
        {
            throw new NotImplementedException();
        }

        public override bool Update(EventImageModel collection)
        {
            throw new NotImplementedException();
        }

        internal async Task<bool> AddImageRange(List<EventImageModel> eventImageList)
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = await _repo.AddImageRange(eventImageList);
                return IsSuccess;
            }
            catch (Exception)
            {
                return IsSuccess;
            }
        }
        internal async Task<bool> DeleteImages(string iD, EventModel eventID, List<EventImageModel> eventImages)
        {
            try
            {
                var FoundData =  eventImages.Where(i => i.ImageName == iD).FirstOrDefault();
                if (FoundData!=null)
                {
                    _repo.Remove(FoundData);
                    await _repo.Save();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception er)
            {
                return false;
            }
        }
    }
}
