using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class AppUsersController : BaseController
    {
        private readonly IAppUserService _appUserService;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public AppUsersController(IAppUserService appUserService,
             IHttpContextAccessor httpContextAccessor,
             Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _appUserService = appUserService;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new UserModelView();
            if (id > 0)
            {
                model = _appUserService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpPost]
        public JsonResult ChangeServerPassWord(int userId, string current_password,string new_password)
        {
            var model = _appUserService.ChangePassWord(userId, current_password, new_password);
            _appUserService.Save();
            return Json(new
            {
                success = model
            });
        }

        [HttpGet]
        public IActionResult ChangePassWord()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new UserModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var dataUser = _appUserService.GetByid(user.id);
                    model = dataUser;
                }
                return View(model);
            }
            return Redirect("/home");
            
        }


        public IActionResult ViewDetail()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new UserModelView();
            if (userString != null)
            {
               var user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var dataUser = _appUserService.GetByid(user.id);
                    model = dataUser;
                }
                
            }
            return View(model);
        }


        [HttpPost]
        public JsonResult UpdateViewDetail(UserModelView UserModelView)
        {
            bool status = false;
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var model = new UserModelView();
            if (userString != null)
            {
                var user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var dataUser = _appUserService.GetByid(user.id);
                    if (dataUser != null)
                    {

                        if (dataUser.email.ToLower().Trim() != UserModelView.email.ToLower().Trim())
                        {
                            bool isEmail = _appUserService.IsMailExist(UserModelView.email.ToLower().Trim());
                            if (isEmail)
                            {
                                return Json(new
                                {
                                    success = false,
                                    isExistEmail = true,
                                    isExistPhone = false,
                                });
                            }
                        }

                        if (dataUser.phone_number.ToLower().Trim() != UserModelView.phone_number.ToLower().Trim())
                        {
                            bool isPhone = _appUserService.IsPhoneExist(UserModelView.phone_number.ToLower().Trim());
                            if (isPhone)
                            {
                                return Json(new
                                {
                                    success = false,
                                    isExistEmail = false,
                                    isExistPhone = true,
                                });
                            }
                        }

                        UserModelView.id = dataUser.id;
                        status = _appUserService.UpdateDetailView(UserModelView);
                        _appUserService.Save();
                        dataUser = _appUserService.GetByid(user.id);
                        _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(dataUser));
                        return Json(new
                        {
                            success = status,
                            isExistEmail = false,
                            isExistPhone = false,
                        });
                    }                   
                }               
            }

           
            return Json(new
            {
                success = status
            });
        }




        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult UploadImageAvatar()
        //{
        //    var files = Request.Form.Files;
        //    if (files != null && files.Count > 0)
        //    {
        //        string folder = _hostingEnvironment.WebRootPath + $@"\avatar";

        //        if (!Directory.Exists(folder))
        //        {
        //            Directory.CreateDirectory(folder);
        //        }
        //        var _lstImageName = new List<string>();

        //        foreach (var itemFile in files)
        //        {
        //            string filePath = Path.Combine(folder, itemFile.FileName);
        //            if (!System.IO.File.Exists(filePath))
        //            {
        //                _lstImageName.Add(itemFile.FileName);
        //                using (FileStream fs = System.IO.File.Create(filePath))
        //                {
        //                    itemFile.CopyTo(fs);
        //                    fs.Flush();
        //                }
        //            }
        //        }                
        //    }
        //    return Json(new
        //    {
        //        success = true
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsEmailExist(string email)
        //{
        //    bool isMail = false;
        //    if (!string.IsNullOrEmpty(email))
        //    {
        //        isMail = _appUserService.IsMailExist(email);
        //    }

        //    return Json(new
        //    {
        //        success = isMail
        //    });
        //}

        //[HttpPost]
        //public JsonResult IsPhoneExist(string phone)
        //{
        //    bool isphone = false;
        //    if (!string.IsNullOrEmpty(phone))
        //    {
        //        isphone = _appUserService.IsPhoneExist(phone);
        //    }

        //    return Json(new
        //    {
        //        success = isphone
        //    }) ;
        //}


        [HttpPost]
        public IActionResult UploadImageAvartar()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            if (!string.IsNullOrEmpty(userString))
            {
                var model = new UserModelView();
                var user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (user != null)
                {
                    var dataUser = _appUserService.GetByid(user.id);
                    if (dataUser != null)
                    {
                        var _lstImageName = new List<string>();
                        var files = Request.Form.Files;
                        if (files != null && files.Count > 0)
                        {
                            var imageFolder = $@"\avartar\";

                            string folder = _hostingEnvironment.WebRootPath + imageFolder;

                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                            }


                            foreach (var itemFile in files)
                            {
                                string fileNameFormat = Regex.Replace(itemFile.FileName.ToLower(), @"\s+", "");
                                string filePath = Path.Combine(folder, fileNameFormat);
                                if (!System.IO.File.Exists(filePath))
                                {
                                    _lstImageName.Add(fileNameFormat);
                                    using (FileStream fs = System.IO.File.Create(filePath))
                                    {
                                        itemFile.CopyTo(fs);
                                        fs.Flush();
                                    }
                                }
                            }
                        }
                      

                        if (_lstImageName != null && _lstImageName.Count > 0)
                        {
                            foreach (var item in _lstImageName)
                            {
                                var userModelView = new UserModelView();
                                userModelView.avatar = item;                               
                                userModelView.id = dataUser.id;
                                _appUserService.UpdateAvartar(userModelView);
                                _appUserService.Save();
                                user.avatar = item;
                            }

                        }

                        _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(user));
                    }
                }
            }
            return Json(new
            {
                success = true
            });

        }





        [HttpPost]
        public JsonResult Add(UserModelView UserModelView)
        {            
            bool isMailExist = false;
            bool isPhoneExist = false;
            if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.email))
            {
                var isMail = _appUserService.IsMailExist(UserModelView.email);
                if (isMail)
                {
                    isMailExist = true;
                }
            }

            if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.phone_number))
            {
                var isPhone = _appUserService.IsPhoneExist(UserModelView.phone_number);
                if (isPhone)
                {
                    isPhoneExist = true;
                }
            }

            if (!isMailExist && !isPhoneExist)
            {
                var result = _appUserService.Add(UserModelView);
                _appUserService.Save();
                return Json(new
                {
                    success = result
                });
            }
            return Json(new
            {
                success = false,
                isMailExist = isMailExist,
                isPhoneExist = isPhoneExist
            });

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _appUserService.UpdateStatus(id, status);
                _appUserService.Save();
                return Json(new
                {
                    success = model
                });
            }
            return Json(new
            {
                success = false
            });

        }

        [HttpPost]
        public JsonResult Update(UserModelView UserModelView)
        {

            //bool isMailExist = false;
            //bool isPhoneExist = false;
            //if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.email))
            //{
            //    var isMail = _appUserService.IsMailExist(UserModelView.email);
            //    if (isMail)
            //    {
            //        isMailExist = true;
            //    }
            //}

            //if (UserModelView != null && !string.IsNullOrEmpty(UserModelView.phone_number))
            //{
            //    var isPhone = _appUserService.IsPhoneExist(UserModelView.phone_number);
            //    if (isPhone)
            //    {
            //        isPhoneExist = true;
            //    }
            //}

            var result = _appUserService.Update(UserModelView);
            _appUserService.Save();
            return Json(new
            {
                success = result
            });



        }

        //[HttpPost]
        //public JsonResult AddUserRoles (int userId, int[] rolesId)
        //{
        //    try
        //    {
        //        _appUserRoleService.AddAppUserRole(userId, rolesId);

        //        return Json(new
        //        {
        //            success = true
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new
        //        {
        //            success = false
        //        });
        //    }

        //}

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _appUserService.Deleted(id);
            _appUserService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(UserModelViewSearch colorViewModelSearch)
        {
            var data = _appUserService.GetAllPaging(colorViewModelSearch);
            return Json(new { data = data });
        }
        //[HttpGet]
        //public JsonResult GetAll()
        //{
        //    var data = _appUserService.GetAll();
        //    return Json(new { Data = data });
        //}
    }
}
