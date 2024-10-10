
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TECH.Service
{
    public interface IAppUserService
    {
        PagedResult<UserModelView> GetAllPaging(UserModelViewSearch userModelViewSearch);
        UserModelView GetByid(int id);
        UserModelView GetByUser(string emailOrPhone);
        int Add(UserModelView view);
        bool Update(UserModelView view);
        bool Deleted(int id);
        void Save();
        UserModelView UserLogin(UserModelView userModelView);
        bool UpdateDetailView(UserModelView view);
        UserModelView AppUserLogin(string userName,string passWord);
        UserModelView GetUserForEmailCode(string email, string code);
        void UpdateAvartar(UserModelView view);
        bool ChangePassWord(int userId, string current_password, string new_password, bool isCofirm = false);

        bool IsMailExist(string mail);
        bool IsPhoneExist(string phone);
        bool UpdateStatus(int id, int status);
        int GetCount();
        List<UserModelView> GetUserSearch(string search);
        bool UpdateCode(UserModelView view);
    }

    public class AppUserService : IAppUserService
    {
        private readonly IUsersRepository _appUserRepository;
        private IUnitOfWork _unitOfWork;
        public AppUserService(IUsersRepository appUserRepository,
            IUnitOfWork unitOfWork)
        {
            _appUserRepository = appUserRepository;
            _unitOfWork = unitOfWork;
        }
        public bool UpdateCode(UserModelView view)
        {
            try
            {
                var dataServer = _appUserRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.code = view.code;
                    _appUserRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }


        public UserModelView GetUserForEmailCode(string email, string code)
        {
            var data = _appUserRepository.FindAll().Where(u => u.email == email.Trim() && u.code == code).FirstOrDefault();
            if (data != null)
            {
                var model = new UserModelView()
                {
                    id = data.id,
                    full_name = data.full_name,
                    avatar = data.avatar,
                    phone_number = data.phone_number,
                    email = data.email,
                    address = data.address,
                    role = data.role,
                    password = data.password,
                    status = data.status,
                    register_date = data.register_date
                };
                return model;
            }
            return null;
        }

        public bool ChangePassWord(int userId, string current_password, string new_password,bool isCofirm = false)
        {
            var modelUser = _appUserRepository.FindAll().Where(u => u.id == userId).FirstOrDefault();
            bool status = false;
            if (modelUser != null)
            {
                if (modelUser.password == current_password)
                {
                    modelUser.password = new_password;
                    if (isCofirm == true)
                    {
                        modelUser.code = "";
                    }
                    _appUserRepository.Update(modelUser);
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            else
            {
                status = false;
            }
            return status;
        }

        public int GetCount()
        {
            int count = 0;
            count = _appUserRepository.FindAll().Count();
            return count;
        }

        public bool IsMailExist(string mail)
        {
            var data = _appUserRepository.FindAll().Any(p=>p.email == mail);
            return data;
        }
        public bool IsPhoneExist(string phone)
        {
            var data = _appUserRepository.FindAll().Any(p => p.phone_number == phone);
            return data;
        }
        public UserModelView GetByUser(string emailOrPhone)
        {
            var data = _appUserRepository.FindAll(p => p.email == emailOrPhone && p.status == 0).FirstOrDefault();
            if (data != null)
            {
                var model = new UserModelView()
                {
                    id = data.id,
                    full_name = data.full_name,
                    avatar = data.avatar,
                    phone_number = data.phone_number,
                    email = data.email,
                    address = data.address,
                    role = data.role,
                    password = data.password,
                    status = data.status,
                    register_date = data.register_date
                };
                return model;
            }
            return null;
        }
        public UserModelView GetByid(int id)
        {
            var data = _appUserRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new UserModelView()
                {
                    id = data.id,
                    full_name = data.full_name,
                    avatar = data.avatar,
                    phone_number = data.phone_number,
                    email = data.email,
                    address = data.address,
                    role = data.role,
                    password = data.password,
                    status = data.status,
                    register_date = data.register_date
                };
                return model;
            }
            return null;
        }
        public int Add(UserModelView view)
        {
            try
            {
                if (view != null)
                {
                    var appUser = new Users
                    {
                        full_name = view.full_name,
                        avatar = view.avatar,
                        phone_number  = view.phone_number,
                        email = view.email,
                        address  = view.address,
                        role  = view.role.HasValue ? view.role.Value: 1,
                        password   = view.password,  
                        status = 0,
                        register_date = DateTime.Now
                    };
                    _appUserRepository.Add(appUser);
                    
                    Save();

                    return appUser.id;                    
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(UserModelView view)
        {
            try
            {
                var dataServer = _appUserRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.address = view.address;
                    dataServer.role = view.role.HasValue? view.role.Value: 1;
                    _appUserRepository.Update(dataServer);                                        
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool UpdateDetailView(UserModelView view)
        {
            try
            {
                var dataServer = _appUserRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.address = view.address;
                    dataServer.full_name = view.full_name;
                    dataServer.email = view.email;
                    dataServer.phone_number = view.phone_number;
                    _appUserRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public void UpdateAvartar(UserModelView view)
        {
            try
            {
                var dataServer = _appUserRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.avatar = view.avatar;
                    _appUserRepository.Update(dataServer);
                  
                }
            }
            catch (Exception ex)
            {
                //return false;
            }

        }


        public bool UpdateStatus(int id, int status)
        {
            try
            {
                var dataServer = _appUserRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _appUserRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _appUserRepository.FindById(id);
                if (dataServer != null)
                {
                    _appUserRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public UserModelView AppUserLogin(string userName, string passWord)
        {
            var data = _appUserRepository.FindAll().Where(u=>(u.email == userName || u.phone_number == userName) && u.status == 0).FirstOrDefault();
            if (data != null && data.password == passWord)
            {
                var model = GetByid(data.id);
                if (model != null)
                {
                    return model;
                }
            }
            return null;
        }

        public List<UserModelView> GetUserSearch(string search)
        {
            var query = _appUserRepository.FindAll();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.full_name.ToLower().Trim().Contains(search.ToLower().Trim()) || c.phone_number.ToLower().Trim().Contains(search.ToLower().Trim()));
                var data = query.Select(c => new UserModelView()
                {
                    full_name = (!string.IsNullOrEmpty(c.full_name) ? c.full_name : ""),
                    id = c.id,
                    phone_number = !string.IsNullOrEmpty(c.phone_number) ? c.phone_number : "",
                    email = !string.IsNullOrEmpty(c.email) ? c.email : "",
                    address = !string.IsNullOrEmpty(c.address) ? c.address : "",
                    role = c.role,
                    password = c.password,
                    status = c.status,
                    register_date = c.register_date,
                }).ToList();
                return data;
            }
            return null;
        }

        public PagedResult<UserModelView> GetAllPaging(UserModelViewSearch userModelViewSearch)
        {
            try
            {
                var query = _appUserRepository.FindAll();

                if (userModelViewSearch.role.HasValue)
                {
                    query = query.Where(c => c.role == userModelViewSearch.role.Value);
                }
                
                if (!string.IsNullOrEmpty(userModelViewSearch.name))
                {
                    query = query.Where(c => c.full_name.ToLower().Contains(userModelViewSearch.name.ToLower()) || c.email.ToLower().Contains(userModelViewSearch.name.ToLower()) || c.phone_number.ToLower().Contains(userModelViewSearch.name.ToLower()));
                }

                int totalRow = query.Count();
                query = query.Skip((userModelViewSearch.PageIndex - 1) * userModelViewSearch.PageSize).Take(userModelViewSearch.PageSize);
                var data = query.OrderByDescending(p => p.id).Select(c => new UserModelView()
                {
                    full_name = (!string.IsNullOrEmpty(c.full_name) ? c.full_name : ""),
                    id = c.id,
                    phone_number = !string.IsNullOrEmpty(c.phone_number) ? c.phone_number : "",
                    email = !string.IsNullOrEmpty(c.email) ? c.email : "",
                    address = !string.IsNullOrEmpty(c.address) ? c.address : "",
                    role = c.role,
                    password = c.password,
                    status = c.status,
                    register_date = c.register_date,
                 }).ToList();
              
                var pagingData = new PagedResult<UserModelView>
                {
                    Results = data,
                    CurrentPage = userModelViewSearch.PageIndex,
                    PageSize = userModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public UserModelView UserLogin(UserModelView userModelView)
        {
            var data = _appUserRepository.FindAll().Where(u => u.phone_number.Trim().ToLower() == userModelView.phone_number.Trim().ToLower() ||
                                                          u.email == userModelView.email).FirstOrDefault();

            if (data != null && data.password != null && userModelView.password != null &&
                data.password.Trim() == userModelView.password.Trim())
            {
                var dataServer = new UserModelView()
                {
                    id = data.id,
                    full_name = data.full_name,
                };               
                return dataServer;
            }
            return null;
            
        }

        public List<UserModelView> GetAll()
        {
            var data = _appUserRepository.FindAll().Select(c => new UserModelView()
            {
                id = c.id,
                full_name = c.full_name,
                //Code = c.Code
            }).ToList();

            return data;
        }

    }
}
