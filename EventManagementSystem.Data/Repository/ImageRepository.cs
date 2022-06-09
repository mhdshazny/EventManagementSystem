using EventManagementSystem.Db;
using EventManagementSystem.Models;
using EventManagementSystem.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository
{
    public class ImageRepository : Repository<EventImageModel>,IImageRepository
    {
        private readonly AppDbContext _db;
        private readonly string _path;

        public ImageRepository(AppDbContext dbContext, IWebHostEnvironment _environment):base(dbContext)
        {
            _db = dbContext;
            _path = _environment.ContentRootPath+"\\wwwroot\\";
        }

        public async Task<bool> AddImage(EventImageModel ImageData)
        {
            bool IsSuccess = false;
            int Changes = 0;
            try
            {
                //CopyFromFile

                string fileName = Path.GetFileName(ImageData.ImageName);
                string fileExtension = Path.GetExtension(ImageData.ImageName);
                //EndCopyFromFile


                //SaveStart

                ImageData.ImageName = fileName;
                ImageData.IsImageAvailable = true;

                _db.Entry(ImageData).State = EntityState.Modified;
                _db.EventImagetData.Add(ImageData);

                Changes = await Save();


                //SavedEnd

                if (Changes>0)
                {
                    IsSuccess = true;
                }
                return IsSuccess;

            }
            catch (Exception er)
            {
                return IsSuccess;
            }

        }
        public async Task<bool> AddImageRange(List<EventImageModel> eventImageList)
        {
            bool IsSuccess = false;
            int Changes = 0;
            try
            {
                //CopyFromFile

                //string fileName = Path.GetFileName(ImageData.ImageName);
                //string fileExtension = Path.GetExtension(ImageData.ImageName);
                //EndCopyFromFile


                //SaveStart


                _db.Entry(eventImageList).State = EntityState.Detached;
                await _db.EventImagetData.AddRangeAsync(eventImageList);


                Changes = await Save();

                if (Changes > 0)
                {
                    IsSuccess = true;
                }
                return IsSuccess;

            }
            catch (Exception er)
            {
                return IsSuccess;
            }
        }









        public IEnumerable<EventImageModel> eventImages(string EventID)
        {


            List<EventImageModel> eventImages = new List<EventImageModel>();

            try
            {
                eventImages = _db.EventImagetData.Where(i=>i.Event.EventID==EventID)
                    .Include(i=>i.Event).ThenInclude(i=>i.EventRecordHanledBy)
                    .ToList();
                return eventImages;
            }
            catch (Exception er)
            {
                return eventImages;
            }

        }

        public async Task<int> Save()
        {
            int Changes = await _db.SaveChangesAsync();

            return Changes;
        }

        public void Update(EventImageModel Entity)
        {
            _db.EventImagetData.Update(Entity);
        }
        public void UpdateRange(List<EventImageModel> Entity)
        {
            _db.EventImagetData.UpdateRange(Entity);
        }

    }
}
