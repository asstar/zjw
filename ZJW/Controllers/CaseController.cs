using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using IBLL;
using BLL;
namespace zjw.Controllers
{
    public class CaseController : BaseController
    {
        //
        // GET: /Case/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CaseCreate()
        {
            Case caseInfo = new Case();
            ViewBag.Case = caseInfo;
            Session["Flag"] = "Create";
            return View("CaseEdit");
        }
        public ActionResult GiftCreate()
        {
            Gift giftInfo = new Gift();
            ViewBag.Money = giftInfo;
            Session["Flag"] = "Create";
            return View("GiftEdit");
        }
	}
}