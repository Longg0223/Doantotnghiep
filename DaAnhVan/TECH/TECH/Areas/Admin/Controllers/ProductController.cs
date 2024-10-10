using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;
using System.Text.RegularExpressions;

namespace TECH.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductsService _productsService;
        private readonly IImagesService _imagesService;
        private readonly IProductsImagesService _productsImagesService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductsService productsService,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
            IImagesService imagesService,
            IProductsImagesService productsImagesService,
            ICategoryService categoryService)
        {
            _productsService = productsService;
            _hostingEnvironment = hostingEnvironment;
            _categoryService = categoryService;
            _imagesService = imagesService;
            _productsImagesService = productsImagesService;
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
            var model = new ProductModelView();
            if (id > 0)
            {
                model = _productsService.GetByid(id);
               
                if (model != null && !string.IsNullOrEmpty(model.name))
                {

                    var productimages = _productsImagesService.GetImageProduct(model.id);
                    if (productimages != null && productimages.Count > 0)
                    {
                        var lstImages = _imagesService.GetImageName(productimages);
                        if (lstImages != null && lstImages.Count > 0)
                        {
                            model.ImageModelView = lstImages;
                        }
                    }

                    if (model.category_id.HasValue && model.category_id.Value > 0)
                    {
                        var category = _categoryService.GetByid(model.category_id.Value);
                        model.categorystr = category.name;
                    }
                    else
                    {
                        model.categorystr = "";
                    }
                   
                }
                else
                {
                    model.categorystr = "Chờ xử lý";
                }

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

  
        [HttpPost]
        public IActionResult UploadImageProduct()
        {
            var files = Request.Form.Files;
            if (files != null && files.Count > 0)
            {
                var imageFolder = $@"\product-image\";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var _lstImageName = new List<string>();
                var productId = Convert.ToInt32(files[0].Name);
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
                    //else
                    //{
                    //    _lstImageName.Add(fileNameFormat);
                    //}
                }
                if (_lstImageName != null && _lstImageName.Count > 0)
                {
                    // remove ảnh đã tồn tại 
                    // xoa product image
                    var lstImage = _productsImagesService.GetImageProduct(productId);
                    _productsImagesService.RemoveProductId(productId);
                     
                    var lstImagesIds = _imagesService.Add(_lstImageName);
                    if (lstImagesIds != null && lstImagesIds.Count > 0)
                    {                       
                        var lstProductImages = lstImagesIds.Select(p => new ProductImageModelView()
                        {
                            product_id = productId,
                            images_id = p
                        }).ToList();
                        if (lstImage != null && lstImage.Count > 0)
                        {
                            foreach (var item in lstImage)
                            {
                                _imagesService.Remove(item);
                            }
                            
                        }
                        _productsImagesService.Add(lstProductImages);
                        _productsImagesService.Save();
                    }
                }
            }
            return Json(new
            {
                success = true
            });
        }

        [HttpPost]
        public IActionResult RemoveImage(List<ImageModelView> images)
        {
            var files = Request.Form.Files;
            if (images != null && images.Count > 0)
            {
                var imageFolder = $@"\product-image\";
                string folder = _hostingEnvironment.WebRootPath + imageFolder;
                foreach (var item in images)
                {
                    string fileNameFormat = Regex.Replace(item.name.ToLower(), @"\s+", "");
                    string filePath = Path.Combine(folder, fileNameFormat);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);                       
                    }
                    if (item.id > 0)
                    {
                        _imagesService.Remove(item.id);
                       
                    }                                                                                 
                }
                _productsImagesService.Remove(images.Select(p=>p.id).ToList());
                _productsService.Save();
            }
            return Json(new
            {
                success = true
            });
        }



        [HttpPost]
        public JsonResult Add(ProductModelView ProductModelView)
        {            
            bool isNameExist = false;
            if (ProductModelView != null && !string.IsNullOrEmpty(ProductModelView.name))
            {
                isNameExist = _productsService.IsProductNameExist(ProductModelView.name);                
            }

            if (!isNameExist)
            {
                var result = _productsService.Add(ProductModelView);
                if (result > 0 )
                {                    
                    return Json(new
                    {
                        success = result,
                        id= result
                    });
                }                
            }
            return Json(new
            {
                success = false,
                isNameExist = isNameExist
            });
        }


        [HttpPost]
        public JsonResult ImagesAdd(List<string> images)
        {            
            if (images != null && images.Count > 0)
            {
                string folder = _hostingEnvironment.WebRootPath + $@"\product-image\";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                var listImageExist = new List<string>();
                foreach (var item in images)
                {
                    string fileNameFormat = Regex.Replace(item.ToLower(), @"\s+", "");
                    string filePath = Path.Combine(folder, fileNameFormat);
                    if (!System.IO.File.Exists(filePath))
                    {
                        listImageExist.Add(item);
                    }
                }
                
                if (listImageExist != null && listImageExist.Count > 0)
                {
                    images = images.Where(i => !listImageExist.Exists(e => e == i)).ToList();
                }
                if (images != null && images.Count > 0)
                {
                    var listIds = _imagesService.Add(images);
                    _imagesService.Save();
                    if (listIds != null && listIds.Count > 0)
                    {
                        return Json(new
                        {
                            success = true,
                            ids = listIds
                        });
                    }
                }
               
            }
           
            return Json(new
            {
                success = false,              
            });
        }


        [HttpGet]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _productsService.UpdateStatus(id, status);
                _productsService.Save();
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
        public JsonResult Update(ProductModelView ProductModelView)
        {


            var result = _productsService.Update(ProductModelView);
            _productsService.Save();
            return Json(new
            {
                success = result
            });

        }



        [HttpPost]
        public JsonResult Delete(int id)
        {
            var result = _productsService.Deleted(id);
            _productsService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(ProductViewModelSearch colorViewModelSearch)
        {
            var data = _productsService.GetAllPaging(colorViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.category_id.HasValue && item.category_id.Value > 0)
                {
                    var  category = _categoryService.GetByid(item.category_id.Value);
                    var productimages = _productsImagesService.GetImageProduct(item.id);
                    if (productimages != null && productimages.Count > 0)
                    {
                       var lstImages =  _imagesService.GetImageName(productimages);
                        if (lstImages != null && lstImages.Count > 0)
                        {
                            item.avatar = lstImages[0].name;
                        }
                    }
                    if (category != null && !string.IsNullOrEmpty(category.name))
                    {
                        item.categorystr = category.name;
                    }
                    else
                    {
                        item.categorystr = "Chờ xử lý";
                    }
                    
                }
                else
                {
                    item.categorystr = "";
                }
                var productImage = _productsImagesService.GetImageProduct(item.id);
                if (productImage != null && productImage.Count > 0)
                {
                    var image = _imagesService.GetImageName(productImage);
                    if (image != null && image.Count > 0)
                    {
                        item.ImageModelView = image;
                    }
                }
                item.trademark = !string.IsNullOrEmpty(item.trademark) ? item.trademark : "";
                item.trademarkStr = !string.IsNullOrEmpty(item.trademark) ? item.trademark : "";
                item.price_sell_str = item.price_sell.HasValue && item.price_sell.Value > 0 ? item.price_sell.Value.ToString("#,###") : "";
                item.price_import_str = item.price_import.HasValue && item.price_import.Value > 0 ? item.price_import.Value.ToString("#,###") : "";
                item.price_reduced_str = item.price_reduced.HasValue && item.price_reduced.Value > 0 ? item.price_reduced.Value.ToString("#,###") : "";
                item.number_import = item.number_import;
                item.cover_type = item.cover_type;
                item.cover_type_str = item.cover_type_str;

                //item.total_product = 10;

            }
            return Json(new { data = data });
        }


        [HttpGet]
        public JsonResult ProductSearch(ProductViewModelSearch productViewModelSearch)
        {
            productViewModelSearch.PageIndex = 1;
            productViewModelSearch.PageIndex = 20;
            var data = _productsService.GetAllPaging(productViewModelSearch);
            foreach (var item in data.Results)
            {
                if (item.category_id.HasValue)
                {
                    var category = _categoryService.GetByid(item.category_id.Value);
                    if (category != null && !string.IsNullOrEmpty(category.name))
                    {
                        item.categorystr = category.name;
                    }
                    else
                    {
                        item.categorystr = "Chờ xử lý";
                    }

                }

            }
            return Json(new { data = data });
        }

        [HttpGet]
        public IActionResult RelatedProducts(int categoryId, int productId)
        {
            var relatedProducts = _productsService.GetProductReLated(categoryId, productId);
            return ViewComponent("RelatedProductsComponent", new { categoryId, productId });
        }

    }
}
