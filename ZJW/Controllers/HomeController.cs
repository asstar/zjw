using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace zjw.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var temp = MenuBar.GetJson(((BaseInfo)Session["User"]).user.ID);
            Session["Menus"] = temp;
            Session["UsePrev"] = false;
            Session["Caretaker"] = null;
            return View();
        }
	}
}