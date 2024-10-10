using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using Microsoft.AspNetCore.Hosting;

namespace TECH.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public CategoryController(ICategoryService categoryService,
             Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _categoryService = categoryService;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new CategoryModelView();
            if (id > 0)
            {
                model = _categoryService.GetByid(id);
            }
            return Json(new
            {
                Data = model
            });
        }

        [HttpGet]
        public JsonResult GetAll()
        {            
            var data = _categoryService.GetAll();
            return Json(new
            {
                Data = data
            });
        }

        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadImage()
        {
            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var imageFolder = $@"\category-image\";

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
        public JsonResult Add(CategoryModelView CategoryModelView)
        {

            bool isCategoryNameExist = false;
            if (CategoryModelView != null && !string.IsNullOrEmpty(CategoryModelView.name))
            {
                isCategoryNameExist = _categoryService.IsCategoryNameExist(CategoryModelView.name);               
            }

            if (!isCategoryNameExist)
            {
                _categoryService.Add(CategoryModelView);
                _categoryService.Save();
                return Json(new
                {
                    success = true
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    isCategoryNameExist = isCategoryNameExist
                });
            }

           

        }

        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _categoryService.UpdateStatus(id, status);
                _categoryService.Save();
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
        public JsonResult Update(CategoryModelView CategoryModelView)
        {
            var result = _categoryService.Update(CategoryModelView);
            _categoryService.Save();
            return Json(new
            {
                success = result
            });

            //bool isCategoryNameExist = false;
            //if (CategoryModelView != null && !string.IsNullOrEmpty(CategoryModelView.name))
            //{
            //    isCategoryNameExist = _categoryService.IsCategoryNameExist(CategoryModelView.name);
            //}


            //if (!isCategoryNameExist)
            //{
            //    var result = _categoryService.Update(CategoryModelView);
            //    _categoryService.Save();
            //    return Json(new
            //    {
            //        success = result
            //    });
            //}
            //else
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        isCategoryNameExist = isCategoryNameExist
            //    });
            //}


        }

     
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _categoryService.Deleted(id);
            _categoryService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(CategoryViewModelSearch categoryViewModelSearch)
        {
            var data = _categoryService.GetAllPaging(categoryViewModelSearch);
            return Json(new { data = data });
        }
    }
}
