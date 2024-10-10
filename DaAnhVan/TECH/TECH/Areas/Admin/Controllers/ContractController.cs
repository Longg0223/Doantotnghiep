using Microsoft.AspNetCore.Mvc;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class ContractController : BaseController
    {
        private readonly IContractsService _contractsService;
        public ContractController(IContractsService contractsService)
        {
            _contractsService = contractsService;
        }
        public IActionResult Index()
        {
            return View();
        }       
        [HttpGet]
        public IActionResult AddOrUpdate()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetById(int id)
        {
            var model = new ContractModelView();
            if (id > 0)
            {
                model = _contractsService.GetById(id);
            }
            return Json(new
            {
                Data = model
            });
        }


        [HttpPost]
        public JsonResult Add(ContractModelView UserModelView)
        {
            _contractsService.Add(UserModelView);
            _contractsService.Save();
            return Json(new
            {
                success = true
            });

        }

        [HttpPost]
        public JsonResult UpdateStatus(int id,int status)
        {
            if (id > 0)
            {
               var  model = _contractsService.Update(id, status);
                _contractsService.Save();
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
            var result = _contractsService.Deleted(id);
            _contractsService.Save();
            return Json(new
            {
                success = result
            });
        }

        [HttpGet]
        public JsonResult GetAllPaging(ContractModelViewSearch colorViewModelSearch)
        {
            var data = _contractsService.GetAllPaging(colorViewModelSearch);
            return Json(new { data = data });
        }
        //[HttpGet]
        //public JsonResult GetAll()
        //{
        //    var data = _contractsService.GetAll();
        //    return Json(new { Data = data });
        //}
    }
}
