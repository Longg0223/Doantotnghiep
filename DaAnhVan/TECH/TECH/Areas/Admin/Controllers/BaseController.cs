using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize]
    public class BaseController : Controller
    {
        //public IHttpContextAccessor _httpContextAccessor;
        //public BaseController()
        //{

        //}

        ////public BaseController(IHttpContextAccessor httpContextAccessor)
        ////{
        ////    _httpContextAccessor = httpContextAccessor;

        ////}
        //public IActionResult Index1(IHttpContextAccessor _httpContextAccessor)
        //{
        //    //_httpContextAccessor = new IHttpContextAccessor();
        //    var UserInfo = _httpContextAccessor.HttpContext.Session.GetString("UserInfo");
        //    string url = "";
        //    if (UserInfo == null)
        //        url = "/home";

        //    return View(url);
        //    // return RedirectToPage("/home");

        //}
    }
}
