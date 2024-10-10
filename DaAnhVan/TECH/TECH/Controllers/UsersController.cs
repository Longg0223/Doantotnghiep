using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using System.Net.Mail;
using TECH.Service;

namespace TECH.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAppUserService _appUserService;
        public IHttpContextAccessor _httpContextAccessor;
        public UsersController(IAppUserService appUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            _appUserService = appUserService;
            _httpContextAccessor = httpContextAccessor;
        }  
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
           
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUserForGot(string email)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(email))
            {
                var data = _appUserService.GetByUser(email);
                if (data != null)
                {
                    string code = DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + data.id.ToString();
                    SendMail(email, code);
                    _httpContextAccessor.HttpContext.Session.SetString("EmailComfirm", email);

                    data.code = code;
                    _appUserService.UpdateCode(data);
                    _appUserService.Save();
                    status = true;
                }
            }
            return Json(new
            {
                success = status
            });
        }

        public IActionResult ChangeNewPassWord()
        {
            return View();
        }


        [HttpPost]
        public JsonResult ChangeNewServerPassWord(string newpassword)
        {
            bool status = false;
            if (!string.IsNullOrEmpty(newpassword))
            {
                var data = _httpContextAccessor.HttpContext.Session.GetString("UserComfirmEd");

                if (data != null)
                {
                   var  user = JsonConvert.DeserializeObject<UserModelView>(data);
                    var model = _appUserService.ChangePassWord(user.id, user.password, newpassword,true);
                    _appUserService.Save();

                    _httpContextAccessor.HttpContext.Session.Remove("UserComfirmEd");

                  var isComfirmEd =  _httpContextAccessor.HttpContext.Session.GetString("EmailComfirm");
                    if (!string.IsNullOrEmpty(isComfirmEd))
                    {
                        _httpContextAccessor.HttpContext.Session.Remove("EmailComfirm");
                    }

                    return Json(new
                    {
                        success = model
                    });
                }
            
            }
            return Json(new
            {
                success = status
            });
        }



        [HttpGet]
        public JsonResult CheckAccuracy(string codeaccuracy)
        {
            bool status = false;
            var email = _httpContextAccessor.HttpContext.Session.GetString("EmailComfirm");
            if (!string.IsNullOrEmpty(codeaccuracy) && !string.IsNullOrEmpty(email))
            {
                var data = _appUserService.GetUserForEmailCode(email, codeaccuracy);
                if (data != null)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("UserComfirmEd", JsonConvert.SerializeObject(data));
                    status = true;
                }
            }
            return Json(new
            {
                success = status
            });
        }




        public void SendMail(string email,string code)
        {

            var html = "<div style=\"background-color:#f4f4f4;margin:0!important;padding:0!important\">"+
   "<div style=\"display:none;font-size:1px;color:#fefefe;line-height:1px;font-family:'Lato',Helvetica,Arial,sans-serif;max-height:0px;max-width:0px;opacity:0;overflow:hidden\">" +
   "</div>" +
   "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">" +
     " <tbody>" +
        " <tr> " +
           "<td bgcolor=\"#FFA73B\" align=\"center\">" +
              "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px\">" +
                "<tbody>" +
                     "<tr>" +
                       " <td align=\"center\" valign=\"top\" style=\"padding:40px 10px 40px 10px\"> </td>" +
                     "</tr> " +
                  "</tbody> " +
               "</table> " +
            "</td> " +
        " </tr> " +
        " <tr> " +
           " <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding:30px 10px 0px 10px\">" +
             "  <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px\">" +
                 " <tbody>" +
                  " <tr>" +
                      "  <td bgcolor=\"#ffffff\" align=\"center\" style=\"padding:20px 30px 40px 30px;color:#666666;font-family:'Lato',Helvetica,Arial,sans-serif;font-size:18px;font-weight:400;line-height:25px\">" +
                         "  <p style=\"margin:0\"> Mã xác thực của bạn là:" +
                          "    <span>" +
                             " </span>" +
                          " </p>" +
                          " <h4>"+ code + "</h4>" +
                          " <p></p>" +
                      "  </td>" +
                    " </tr>" +
                "  </tbody>" +
              " </table>" +
            "</td>" +
        " </tr>" +
        " <tr>" +
           "  <td bgcolor=\"#f4f4f4\" align=\"center\" style=\"padding:30px 10px 0px 10px\">" +
               " <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width:600px\">" +
                "  <tbody>" +
                "     <tr>" +
                    "    <td bgcolor=\"#FFECD1\" align=\"center\" style=\"padding:30px 30px 30px 30px;border-radius:4px 4px 4px 4px;color:#666666;font-family:'Lato',Helvetica,Arial,sans-serif;font-size:18px;font-weight:400;line-height:25px\">" +
                       "    <h2 style=\"font-size:20px;font-weight:400;color:#111111;margin:0\"></h2>" +
                       "    <p style=\"margin:0\"><a href=\"#m_7898227097373759377_\" style=\"color:#ffa73b\"></a></p>" +
                       " </td>" +
                    " </tr>" +
                "  </tbody>" +
              " </table>" +
            "</td>" +
       "  </tr>" +
     " </tbody>" +
  " </table>" +
 " <div class=\"yj6qo\"></div>" +
  " <div class=\"adL\"></div>" +
"</div>";


            MailMessage mail = new MailMessage();
            mail.To.Add(email.Trim());
            mail.From = new  MailAddress("quanghuynh2100@gmail.com");
            mail.Subject = "Xác Thực Tài Khoản";
            mail.Body = html;
            mail.IsBodyHtml = true;
            mail.Sender = new MailAddress("quanghuynh2100@gmail.com");
            SmtpClient smtp = new SmtpClient();
            smtp.Port =587;
            smtp.EnableSsl =true;
            smtp.UseDefaultCredentials =false;
            smtp.Host ="smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential("quanghuynh2100@gmail.com", "jrhnhzhmkfwsxgcm");
            smtp.Send(mail);
        }

        public IActionResult Accuracy()
        {
            var email = _httpContextAccessor.HttpContext.Session.GetString("EmailComfirm");
            if (!string.IsNullOrEmpty(email))
            {
                var data = new UserModelView()
                {
                    email = email,
                };
                return View(data);
            }
            return Redirect("/home");
            
        }

        [HttpPost]
        public JsonResult AppLogin(string userName,string passWord)
        {
            var result = _appUserService.AppUserLogin(userName, passWord);
            
            if (result != null)
            {
                _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(result));

                return Json(new
                {
                    success = result
                });
            }
            return Json(new
            {
                success = false
            });
        }


        [HttpPost]
        public JsonResult AddRegister(UserModelView UserModelView)
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
                if (result > 0)
                {
                    var _user = _appUserService.GetByid(result);
                    _httpContextAccessor.HttpContext.Session.SetString("UserInfor", JsonConvert.SerializeObject(_user));
                }
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
        public IActionResult Profile()
        {
            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                if (!string.IsNullOrEmpty(user.address) && user.address != "null")
                {
                    user.address = user.address;
                }
                else
                {
                    user.address = "";
                }
                return View(user);
            }
            return Redirect("/home");
           
        }

        public JsonResult AppLogin(UserModelView UserModelView)
        {
            var result = _appUserService.Add(UserModelView);
            _appUserService.Save();
            return Json(new
            {
                success = result
            });

        }

        public IActionResult LogOut()
        {

            var userString = _httpContextAccessor.HttpContext.Session.GetString("UserInfor");
            var user = new UserModelView();
            if (userString != null)
            {
                user = JsonConvert.DeserializeObject<UserModelView>(userString);
                _httpContextAccessor.HttpContext.Session.Remove("UserInfor");
            }

            return Redirect("/home");

        }

        public IActionResult ChangePass()
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
    }
}
