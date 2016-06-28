using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
namespace zjw
{
    public class BaseController : Controller
    {
        protected string hostUrl = "";
        /// <summary>
        /// Action执行前判断
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // url
            this.hostUrl = "http://" + this.Request.Url.Host;
            this.hostUrl += this.Request.Url.Port.ToString() == "80" ? "" : ":" + this.Request.Url.Port;
            this.hostUrl += this.Request.ApplicationPath;

            if (Session["UserName"] == null)
            {
                filterContext.Result = RedirectToAction("TimeOut", "Login");
            }

            base.OnActionExecuting(filterContext);

        }

    }
}