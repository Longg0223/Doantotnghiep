using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TECH.Areas.Admin.Controllers
{
    public class PostController : BaseController
    {
        private readonly IPostsService _postsService;
        private readonly IAppUserService _appUserService;
        public IHttpContextAccessor _httpContextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public PostController(IPostsService postsService,
            IAppUserService appUserService,
            IHttpContextAccessor httpContextAccessor,
        Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _postsService = postsService;
            _hostingEnvironment = hostingEnvironment;
            _appUserService = appUserService;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddView()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new PostModelView();
            if (id > 0)
            {
                model = _postsService.GetByid(id);
            }
            return Json(new
            {
                Data = model
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
        //        isMail = _postsService.IsMailExist(email);
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
        //        isphone = _postsService.IsPhoneExist(phone);
        //    }

        //    return Json(new
        //    {
        //        success = isphone
        //    }) ;
        //}

        [HttpPost]
        public IActionResult UploadImageProduct()
        {
            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var imageFolder = $@"\product-post\";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var _lstImageName = new List<string>();

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
            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public JsonResult Add(PostModelView PostModelView)
        {            
            bool isNameExist = false;
            if (PostModelView != null && !string.IsNullOrEmpty(PostModelView.title))
            {
                isNameExist = _postsService.IsNameExist(PostModelView.title);                
            }
            if (!isNameExist)
            {
                var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
                var user = new UserModelView();
                if (userString != null)
                {
                    user = JsonConvert.DeserializeObject<UserModelView>(userString);
                }
                if (user != null)
                {
                    PostModelView.author = user.id;
                }

                var result = _postsService.Add(PostModelView);
                _postsService.Save();
                return Json(new
                {
                    success = result
                });
            }
            return Json(new
            {
                success = false,
                isNameExist = isNameExist
            });

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _postsService.UpdateStatus(id, status);
                _postsService.Save();
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
        public JsonResult Update(PostModelView PostModelView)
        {

            //bool isNameExist = false;
            //if (PostModelView != null && !string.IsNullOrEmpty(PostModelView.name))
            //{
            //    isNameExist = _postsService.IsProductNameExist(PostModelView.name);
            //}

            //if (!isNameExist)
            //{
            //    var result = _postsService.Update(PostModelView);
            //    _postsService.Save();
            //    return Json(new
            //    {
            //        success = result
            //    });
            //}
            //return Json(new
            //{
            //    success = false,
            //    isNameExist = isNameExist
            //});

            var result = _postsService.Update(PostModelView);
            _postsService.Save();
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
            var result = _postsService.Deleted(id);
            _postsService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(PostsViewModelSearch postsViewModelSearch)
        {

            if (postsViewModelSearch != null && !string.IsNullOrEmpty(postsViewModelSearch.name))
            {
                var lstAuthor = _appUserService.GetUserSearch(postsViewModelSearch.name);
                if (lstAuthor != null && lstAuthor.Count > 0)
                {
                    postsViewModelSearch.author_ids = lstAuthor.Select(u => u.id).ToList();
                }
                
            }

            var data = _postsService.GetAllPaging(postsViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.author.HasValue)
                {
                    var  appUser = _appUserService.GetByid(item.author.Value);
                    if (appUser != null && !string.IsNullOrEmpty(appUser.full_name))
                    {
                        item.author_name = appUser.full_name;
                    }                    
                }

            }
            return Json(new { data = data });
        }
        //[HttpGet]
        //public JsonResult GetAll()
        //{
        //    var data = _postsService.GetAll();
        //    return Json(new { Data = data });
        //}
    }
}
