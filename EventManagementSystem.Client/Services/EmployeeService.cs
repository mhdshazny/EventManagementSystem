using AutoMapper;
using EventManagementSystem.Client.Services.IService;
using EventManagementSystem.Client.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EventManagementSystem.Client.Services
{
    public class EmployeeService
    {
        private IConfiguration _config { get; }
        public readonly EmployeeAPIClient _empAPI;
        private readonly IMapper _mapper;

        public EmployeeService(IConfiguration config, IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
            _empAPI = new EmployeeAPIClient(_config["ApiConfigs:EventManagement:Uri"], new HttpClient());
        }

        public async Task<bool> AddAsync(EmployeeViewModel collection)
        {
            try
            {
                bool status = true;
                EmployeeModel MappedData = _mapper.Map<EmployeeModel>(collection);
                MappedData.EmpPassWord = EncryptPassword(MappedData.EmpPassWord);
                if (MappedData.EmpPassWord == null)
                {
                    status = false;
                }
                if (status)
                {
                    var Data = await _empAPI.AddAsync(MappedData);
                }
                return status;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                bool status = true;
                await _empAPI.DeleteAsync(id);
                return status;
            }
            catch (Exception er)
            {
                return false;
            }
        }

        public async Task<EmployeeViewModel> Find(string id)
        {
            EmployeeViewModel VM = new EmployeeViewModel();
            try
            {
                
                EmployeeModel Data = await _empAPI.SearchAsync(id);
                if (Data!=null) 
                    VM = _mapper.Map<EmployeeViewModel>(Data);
                
                return VM;
            }
            catch (Exception er)
            {
                return VM;
            }
        }

        public async Task<List<EmployeeViewModel>> GetList()
        {
            List<EmployeeViewModel> VM = new List<EmployeeViewModel>();
            try
            {

                var Data = await _empAPI.GetAllAsync();

                if (Data.Count != 0)
                {
                    foreach (EmployeeModel item in Data)
                    {
                        EmployeeViewModel temp = _mapper.Map<EmployeeViewModel>(item);
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
                var NewID = await _empAPI.NewIDAsync();
                return NewID.NewID;
            }
            catch (Exception er)
            {
                return null;
            }
        }

        public async Task<bool> Update(EmployeeViewModel collection)
        {
            try
            {
                EmployeeModel MappedData = _mapper.Map<EmployeeModel>(collection);
                var Result = await _empAPI.UpsertAsync(MappedData);

                if (Result!=null)
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

        internal async Task<EmployeeDetailsViewModel> FindDetails(string id)
        {
            EmployeeDetailsViewModel VM = new EmployeeDetailsViewModel();
            try
            {
                ICollection<EventModel> EventLi = await _empAPI.EmpeventAsync(id);

                List<EventViewModel> EventVM = new List<EventViewModel>();
                foreach (EventModel item in EventLi)
                {
                    EventVM.Add(_mapper.Map<EventViewModel>(item));
                }
                VM.EventVMList = EventVM;

                EmployeeModel EmployeeModel = await _empAPI.SearchAsync(id);
                VM.EmployeeVM = _mapper.Map<EmployeeViewModel>(EmployeeModel);

                return VM;

            }
            catch (Exception er)
            {
                return VM;
            }
        }

        internal async Task<EmployeeViewModel> VerifyUser(string UserName, string Password)
        {
            try
            {
                EmployeeModel result = await _empAPI.VerifyUserAsync(UserName, Password);
                if (result != null)
                {
                    EmployeeViewModel employeeViewModel = _mapper.Map<EmployeeViewModel>(result);
                    return employeeViewModel;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        internal string EncryptPassword(string Password)
        {
            try
            {
                string PasswordHash = BCrypt.Net.BCrypt.HashPassword(Password);
                return PasswordHash;
            }
            catch (Exception)
            {
                return null;
            }

        }

        internal bool VerifyPassword(string Password,string PasswordHash)
        {
            bool IsVerified = false;
            try
            {

                IsVerified = BCrypt.Net.BCrypt.Verify(Password,PasswordHash);
                return IsVerified;
            }
            catch (Exception)
            {
                return IsVerified;
            }

        }
    }
}
