using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zjw.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            Session["Menus"] = MenuBar.GetJson(new Guid("266e2696-bf31-4a32-865e-442e315e5f00"));
            return View();
        }
	}
}