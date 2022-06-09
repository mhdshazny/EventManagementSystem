using AutoMapper;
using EventManagementSystem.Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

namespace EventManagementSystem.Client.Services
{
    public class EventService
    {
        private IConfiguration _config { get; }
        public readonly EventAPIClient _eventAPI;
        public readonly EmployeeAPIClient _employeeAPI;
        public readonly EventImageService _imageService;
        private readonly IMapper _mapper;

        public EventService(IConfiguration config, IMapper mapper, IWebHostEnvironment environment)
        {
            _config = config;
            _mapper = mapper;
            _eventAPI = new EventAPIClient(_config["ApiConfigs:EventManagement:Uri"], new HttpClient());
            _employeeAPI = new EmployeeAPIClient(_config["ApiConfigs:EventManagement:Uri"], new HttpClient());
            _imageService = new EventImageService(config,mapper,environment);
        }


        public async Task<bool> AddAsync(EventViewModel collection)
        {
            try
            {
                bool status = true;
                EmployeeModel HandlingEmployee = await _employeeAPI.SearchAsync(collection.EventRecordHanledBy.ID);
                collection.EventRecordHanledBy = _mapper.Map<EmployeeViewModel>(HandlingEmployee);
                //MappedData.EventRecordHanledBy = await _employeeAPI.SearchAsync(collection.EventRecordHanledBy.ID);

                //ImageAdd
                EventImagesViewModel eventImage = new EventImagesViewModel()
                {
                    Event =collection,
                    ImageFiles= collection.Images,
                    //ImageName = collection.Image,
                    IsImageAvailable = true
                };

                EventModel MappedData = _mapper.Map<EventModel>(collection);


                await _eventAPI.AddAsync(MappedData);

                //if (Data == null) status = false;



                status = await _imageService.SaveToAzure(eventImage);

                if (status)
                {
                    EventModel mappedEvent = _mapper.Map<EventModel>(eventImage.Event);


                    //List<EventImageModel> ImageList = new List<EventImageModel>();
                    foreach (FormFile item in eventImage.ImageFiles)
                    {
                        EventImageModel tempEventImageModel = new EventImageModel()
                        {
                            Event = mappedEvent,
                            ImageName = item.FileName,
                            IsImageAvailable = true
                        };

                        //ImageList.Add(tempEventImageModel);
                        var result = await _eventAPI.AddImageAsync(tempEventImageModel);

                    }



                    //EventImageModel FinalObj = _mapper.Map<EventImageModel>(ImageData);
                    //FinalObj.ImageName = fileName;

                    //var result = _eventAPI.AddImageRangeAsync(ImageList);

                }



                return status;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        internal async Task<List<EventViewModel>> GetAllPublicEventsAsync()
        {
            List<EventViewModel> VM = new List<EventViewModel>();
            try
            {
                ICollection<EventModel> Model = await _eventAPI.GetPublicEventsAsync();
                foreach (EventModel item in Model)
                {
                    EventViewModel tempVM = _mapper.Map<EventViewModel>(item);
                    var imageModel = await _eventAPI.EventImageAsync(item.EventID);
                    //tempVM.Image = imageModel.FirstOrDefault().ImageName;

                    VM.Add(tempVM);
                    
                }

                return VM;
            }
            catch (Exception er)
            {
                return VM;
            }
        }

        internal async Task<ViewAllCommentViewModel> GetCommentsVM(string eventId)
        {
            ViewAllCommentViewModel VM = new ViewAllCommentViewModel();

            try
            {

                ICollection<EventCommentsModel> TempData = await _eventAPI.EventCommentsAsync();
                TempData = TempData.Where(i => i.EventID.EventID.Equals(eventId)).ToList();

                List<EventCommentViewModel> CommentsVM = new List<EventCommentViewModel>();
                EventModel eventModel = await _eventAPI.SearchAsync(eventId);
                EventViewModel eventViewModel = _mapper.Map<EventViewModel>(eventModel);
                foreach (EventCommentsModel item in TempData)
                {
                    EventCommentViewModel obj = _mapper.Map<EventCommentViewModel>(item);
                    CommentsVM.Add(obj);
                }
                VM.CommentsList = CommentsVM;
                VM.EventID = eventViewModel;

                return VM;
            }
            catch (Exception er)
            {
                return VM;
            }
        }

        internal async Task<bool> AddCommentAsync(EventCommentViewModel model)
        {
            bool status = false;
            try
            {
                EventCommentsModel MappedData = _mapper.Map<EventCommentsModel>(model);
                var Data = await _eventAPI.Add2Async(MappedData);
                return true;
            }
            catch (Exception er)
            {
                return status;
            }
        }

        internal async Task<EventCommentViewModel> ReadyCommentVM(string eventId,string EmpID)
        {
            EventCommentViewModel VM = new EventCommentViewModel();
            try
            {

                EventModel EventData = await _eventAPI.SearchAsync(eventId);
                EmployeeModel EmpData = await _employeeAPI.SearchAsync(EmpID);
                if (EventData != null)
                {
                    VM.EventID = _mapper.Map<EventViewModel>(EventData);
                    VM.CommentedUser = _mapper.Map<EmployeeViewModel>(EmpData);
                }

                return VM;
            }
            catch (Exception er)
            {
                return VM;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                bool status = true;
                await _eventAPI.DeleteAsync(id);
                return status;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public async Task<EventViewModel> Find(string id)
        {
            EventViewModel VM = new EventViewModel();
            try
            {

                EventModel Data = await _eventAPI.SearchAsync(id); 
                List<string> ImagesNames = new List<string>();

                if (Data != null)
                {
                    VM = _mapper.Map<EventViewModel>(Data);
                    var imageModel = await _eventAPI.EventImageAsync(Data.EventID);
                    //VM.Image = imageModel.FirstOrDefault().ImageName;

                    if (imageModel.Count > 0)
                    {
                        foreach (var file in imageModel)
                        {
                            string data = _imageService.GetImageFormFile(file);
                            if (data != null)
                            {
                                ImagesNames.Add(data);
                            }
                        }
                    }
                    else
                        VM.IsImagesAvailable = false;


                }

                VM.ImageNameList = ImagesNames;


                return VM;
            }
            catch (Exception er)
            {
                return VM;
            }
        }



        public async Task<List<EventViewModel>> GetList()
        {
            List<EventViewModel> VM = new List<EventViewModel>();
            try
            {

                var Data = await _eventAPI.EventAsync();

                if (Data.Count != 0)
                {
                    foreach (EventModel item in Data)
                    {
                        EventViewModel temp = _mapper.Map<EventViewModel>(item);
                        var imageModel = await _eventAPI.EventImageAsync(item.EventID);


                        //List<EventImagesViewModel> imagesVM = new List<EventImagesViewModel>();
                        //if (imageModel.Count>0)
                        //{
                        //    foreach (var ImgModel in imageModel)
                        //    {
                        //        EventImagesViewModel eventImagesViewModel = _mapper.Map<EventImagesViewModel>(ImgModel);
                        //        //eventImagesViewModel.ImageFile = File.Open(eventImagesViewModel.ImageName,FileMode.Open);
                        //        imagesVM.Add(eventImagesViewModel);
                        //    }                            
                        //}
                        //else temp.Image = "No Images Found";

                        //temp.ViewImages = imagesVM;

                        List<string> ImagesNames = new List<string>();

                        foreach (var file in imageModel)
                        {
                            string data = _imageService.GetImageFormFile(file);
                            if (data != null)
                            {
                                ImagesNames.Add(data);
                            }
                        }


                        temp.ImageNameList = ImagesNames;


                        VM.Add(temp);
                    }
                }

                return VM;
            }
            catch (Exception er)
            {
                return VM;
            }
        }


        public async Task<string> NewID()
        {
            try
            {
                var NewID = await _eventAPI.NewIDAsync();
                return NewID.NewID;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public async Task<bool> Update(EventViewModel collection)
        {
            try
            {
                EventModel MappedData = _mapper.Map<EventModel>(collection);
                MappedData.EventRecordHanledBy = await _employeeAPI.SearchAsync(MappedData.EventRecordHanledBy.Id);
                var Result = await _eventAPI.UpsertAsync(MappedData);

                if (Result != null)
                {
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

        internal async Task<EventViewModel> FindDetails(string id)
        {
            EventViewModel VM = new EventViewModel();
            try
            {
                EventModel EventModel = await _eventAPI.SearchAsync(id);

                VM = _mapper.Map<EventViewModel>(EventModel);
                var imageModel = await _eventAPI.EventImageAsync(EventModel.EventID);
                //VM.Image = imageModel.FirstOrDefault().ImageName;

                return VM;

            }
            catch (Exception er)
            {
                return VM;
            }
        }
        internal async Task<bool> CloseEvent(string id)
        {
             
            bool Result = false;
            try
            {
                var Data = await Find(id);
                if (!Data.Equals(null))
                {
                    Data.Status = "Completed";
                    Result = await Update(Data);
                }
                return Result;
            }
            catch (Exception er)
            {
                return Result;
            }
        }


        ////Image Processing


    }
}
