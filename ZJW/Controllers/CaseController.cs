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
        ICaseService caseService = new CaseService();
        IGiftService giftService = new GiftService();
        //
        // GET: /Case/
        public ActionResult Movie()
        {
            return View();
        }
        public ActionResult CaseCreate()
        {
            Case caseInfo = new Case();
            caseInfo.ID = Guid.NewGuid();
            ViewBag.Case = caseInfo;
            Session["Flag"] = "Create";
            return View("CaseEdit");
        }
        [HttpPost]
        public ActionResult CaseCreate(Case item)
        {

            try
            {
                //item.ID = Guid.NewGuid();
                caseService.Add(item);
                ModelState.Clear();
                return CaseCreate();
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }
        public ActionResult CaseEdit(Guid ID)
        {
            var temp = caseService.Find(ID);
            ViewBag.Case = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult CaseEdit(Case item)
        {
            caseService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult CaseDetails(Guid ID)
        {
            Case caseInfo = caseService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Case = caseInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("CaseEdit");
        }
        [HttpPost]
        public ActionResult CaseDelete(Guid ID)
        {
            caseService.Delete(ID);
            return Content("success");
        }
        public ActionResult GiftCreate()
        {
            Gift giftInfo = new Gift();
            giftInfo.ID = Guid.NewGuid();
            ViewBag.Gift = giftInfo;
            Session["Flag"] = "Create";
            return View("GiftEdit");
        }
        [HttpPost]
        public ActionResult GiftCreate(Gift item)
        {

            try
            {
                //item.ID = Guid.NewGuid();
                giftService.Add(item);
                ModelState.Clear();
                return GiftCreate();
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }
        public ActionResult GiftEdit(Guid ID)
        {
            var temp = giftService.Find(ID);
            ViewBag.Gift = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult GiftEdit(Gift item)
        {
            giftService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult GiftDetails(Guid ID)
        {
            Gift giftInfo = giftService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Gift = giftInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("GiftEdit");
        }
        [HttpPost]
        public ActionResult GiftDelete(Guid ID)
        {
            giftService.Delete(ID);
            return Content("success");
        }
        public ActionResult CaseList()
        {
            return View();
        }
        public JsonResult GetCaseList()
        {
            Originator<ListModel> o = new Originator<ListModel>();
            o.State = getListModel();
            if ((bool)Session["UsePrev"] == false || Session["Caretaker"] == null)
            {

                Caretaker<ListModel> c = new Caretaker<ListModel>();
                c.Memento = o.CreateMemento();
                Session["Caretaker"] = c;
            }
            else
            {
                Caretaker<ListModel> c = (Caretaker<ListModel>)Session["Caretaker"];
                o.SetMemento(c.Memento);
            }
            ListModel info = o.State;
            int total = 0;
            var result = caseService.List(info, "`Case`", ref total);
            total = result.Count();

            var data = new
            {
                total = total,
                rows = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GiftList()
        {
            return View();
        }
        public JsonResult GetGiftList()
        {
            Originator<ListModel> o = new Originator<ListModel>();
            o.State = getListModel();
            if ((bool)Session["UsePrev"] == false || Session["Caretaker"] == null)
            {

                Caretaker<ListModel> c = new Caretaker<ListModel>();
                c.Memento = o.CreateMemento();
                Session["Caretaker"] = c;
            }
            else
            {
                Caretaker<ListModel> c = (Caretaker<ListModel>)Session["Caretaker"];
                o.SetMemento(c.Memento);
            }
            ListModel info = o.State;
            int total = 0;
            var result = giftService.List(info, "`Gift`", ref total);
            total = result.Count();

            var data = new
            {
                total = total,
                rows = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ListModel getListModel()
        {
            ListModel info = new ListModel();
            info.PageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);//Request["page"] == null ? 1 : int.Parse(Request["page"]);
            info.PageSize = Request["rows"] == null ? 30 : int.Parse(Request["rows"]);
            info.Sort = Request["sort"];
            info.Direction = Request["order"];
            info.QueryType = Request["queryType"];
            info.Opt = Request["queryFH"];
            info.KeyWord = Request["keyWord"];
            info.QueryString = "";
            if (info.QueryType != null && info.KeyWord != null && info.Opt != "" && info.QueryType != "" && info.KeyWord != null && info.Opt != "")
            {
                if (info.Opt != "LIKE")
                {
                    info.QueryString = " and " + info.QueryType + " " + info.Opt + " '" + info.KeyWord + "' ";
                }
                else
                {
                    info.QueryString = " and " + info.QueryType + " LIKE '%" + info.KeyWord + "%' ";
                }
            }
            return info;
        }
	}
}