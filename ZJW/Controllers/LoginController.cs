using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using IBLL;
using Model;
namespace zjw.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Login()
        {
            return View();
        }
        public JsonResult CheckUserLogin(Users user)
        {
            using (DBEntities db = new DBEntities())
            {

                var users = from p in db.Users
                            where p.UserName == user.UserName && p.Password == user.Password
                            select p;
                if (users.Count() > 0)
                {
                    user = users.FirstOrDefault();
                    return Json(new { result = "success", content = "" });
                }
                else
                {
                    return Json(new { result = "error", content = "用户名密码错误，请您检查" });
                }
            }
        }

	}
}