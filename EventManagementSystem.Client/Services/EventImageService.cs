using AutoMapper;
using EventManagementSystem;
using EventManagementSystem.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagementSystem.Services
{
    public class EventImageService
    {
        private IConfiguration _config { get; }
        public readonly EventAPIClient _eventAPI;
        public readonly AzureImageUploadService _azureImageUploadService;
        //public readonly EmployeeAPIClient _employeeAPI;
        private readonly IMapper _mapper;
        private readonly string _path;

        public EventImageService(IConfiguration config, IMapper mapper, IWebHostEnvironment _environment)
        {
            _config = config;
            _mapper = mapper;
            _eventAPI = new EventAPIClient(_config["ApiConfigs:EventManagement:Uri"], new HttpClient());
            //_employeeAPI = new EmployeeAPIClient(_config["ApiConfigs:EventManagement:Uri"], new HttpClient());
            _path = _environment.ContentRootPath+"\\wwwroot\\";
            _azureImageUploadService = new AzureImageUploadService(config);

        }

        public async Task<bool> SaveToAzure(EventImagesViewModel ImageData)
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = await _azureImageUploadService.UploadFiles(ImageData.ImageFiles,ImageData.Event.EventID);
                return IsSuccess;
            }
            catch (Exception er)
            {
                throw er;
            }
        }

        public async Task<bool> SaveImage(EventImagesViewModel ImageData)
        {
            bool IsSuccess = true;
            try
            {
                string tempDirectory = _path + "Images\\" + ImageData.Event.EventID;

                //CopyFromFile

                //string fileName = Path.GetFileName(ImageData.ImageName);
                //string fileExtension = Path.GetExtension(ImageData.ImageName);
                //EndCopyFromFile

                //string CopyTo = tempDirectory + "\\" + fileName;
                if (FolderExist(tempDirectory))
                {
                    foreach (FormFile item in ImageData.ImageFiles)
                    {
                        string CopyTo = tempDirectory + "\\" + item.FileName;
                        FileStream fileStream = new FileStream(CopyTo, FileMode.CreateNew);

                        await item.CopyToAsync(fileStream,CancellationToken.None);
                        IsSuccess = true;
                    }

                    //if (IsSuccess)
                    //{
                    //    EventModel mappedEvent = _mapper.Map<EventModel>(ImageData.Event);


                    //    List<EventImageModel> ImageList = new List<EventImageModel>();
                    //    foreach (FormFile item in ImageData.ImageFiles)
                    //    {
                    //        EventImageModel tempEventImageModel = new EventImageModel()
                    //        {
                    //            Event=mappedEvent,
                    //            ImageName= item.FileName,
                    //            IsImageAvailable=true
                    //        };

                    //        ImageList.Add(tempEventImageModel);
                    //    }



                    //    //EventImageModel FinalObj = _mapper.Map<EventImageModel>(ImageData);
                    //    //FinalObj.ImageName = fileName;

                    //    var result = _eventAPI.AddImageRangeAsync(ImageList);

                    //}
                }
                else
                {
                    IsSuccess = CreateFolder(tempDirectory);
                    //IsSuccess = CopyFile(ImageData.ImageName, CopyTo);
                    foreach (FormFile item in ImageData.ImageFiles)
                    {
                        string CopyTo = tempDirectory + "\\" + item.FileName;
                        FileStream fileStream = new FileStream(CopyTo, FileMode.Create);

                        await item.CopyToAsync(fileStream, CancellationToken.None);
                        IsSuccess = true;
                    }

                    //EventModel mappedEvent = _mapper.Map<EventModel>(ImageData.Event);


                    //List<EventImageModel> ImageList = new List<EventImageModel>();
                    //foreach (FormFile item in ImageData.ImageFiles)
                    //{
                    //    EventImageModel tempEventImageModel = new EventImageModel()
                    //    {
                    //        Event = mappedEvent,
                    //        ImageName = item.FileName,
                    //        IsImageAvailable = true
                    //    };

                    //    ImageList.Add(tempEventImageModel);
                    //}

                    //var result = _eventAPI.AddImageRangeAsync(ImageList);

                }

                return IsSuccess;
            }
            catch (Exception er)
            {
                throw er;
            }

        }
        //public bool CopyFile(string filename, string CopyTo)
        //{
        //    //string filename = "Log_" + "FileStreamBasics" + ".txt";
        //    //string path = AppDomain.CurrentDomain.BaseDirectory.ToString();
        //    string BackupPath = "E:\\" + filename;
        //    try
        //    {
        //        File.Copy(filename, CopyTo);
        //        //Console.WriteLine("File Copied Successfully");
        //        return true;

        //    }
        //    catch (IOException er)
        //    {
        //        //Console.WriteLine(er.Message);
        //        return false;
        //    }
        //}

        private bool CreateFolder(string diectory)
        {
            var IsExist = Directory.CreateDirectory(diectory);
            return IsExist.Exists;
        }

        private bool FolderExist(string directory)
        {
            bool IsExist = Directory.Exists(directory);
            return IsExist;
        }


        internal string GetImageFormFile(EventImageModel imageModel)
        {
            try
            {
                //string tempDirectory = _path + "Images\\" + imageModel.Event.EventID;

                //string CopyTo = tempDirectory + "\\" + imageModel.ImageName;
                //FileStream fileStream = new FileStream(CopyTo, FileMode.Open);

                //IFormFile formFile = new FormFile(fileStream,0,0,imageModel.Event.EventID,imageModel.ImageName);
                return imageModel.ImageName;
            }
            catch (Exception er)
            {
                return null;
            }

        }
        internal async Task<bool> DeleteImage(string id, string eventID)
        {
            try
            {
                bool IsSuccess = false;
                IsSuccess = await _eventAPI.DeleteimageAsync(id,eventID);
                if (IsSuccess)
                {
                    bool AzureUpdated = false;
                    AzureUpdated = await _azureImageUploadService.DeleteFile(id,eventID);
                    if (AzureUpdated)
                    {
                        return AzureUpdated;
                    }
                    else
                    {
                        return false;
                    }
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
