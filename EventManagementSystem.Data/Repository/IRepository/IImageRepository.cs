using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.Repository.IRepository
{
    public interface IImageRepository : IRepository<EventImageModel>
    {
        //string GetImageFolderPath(string EventID);

        IEnumerable<EventImageModel> eventImages(string EventID);

        Task<bool> AddImage(EventImageModel ImageData);
        Task<bool> AddImageRange(List<EventImageModel> eventImageList);

        void Update(EventImageModel Entity);
        void UpdateRange(List<EventImageModel> Entity);
        Task<int> Save();
    }
}
