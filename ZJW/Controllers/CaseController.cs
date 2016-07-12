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
        IMasterService masterService = new MasterService();

        IUserService userService = new UserService();
        //
        // GET: /Case/
        public ActionResult Movie()
        {
            return View();
        }
        public ActionResult CaseCreate()
        {
            Master caseInfo = new Master();
            caseInfo.ID = Guid.NewGuid();
            caseInfo.UserID = ((BaseInfo)Session["User"]).user.ID;
            caseInfo.UnderTakenDept = ((BaseInfo)Session["User"]).user.RealName;
            caseInfo.IsDeleted = false;
            caseInfo.MasterType = "案件";
            ViewBag.Master = caseInfo;
            Session["Flag"] = "Create";
            return View("CaseEdit");
        }
        [HttpPost]
        public ActionResult CaseCreate(Master item)
        {


                //item.ID = Guid.NewGuid();
                masterService.Add(item);
                makeCaseUser(item);
                ModelState.Clear();
                return CaseCreate();

        }
        public void makeCaseUser(Master item)
        {
            var masterID = item.ID;
            Users find = userService.FindByMasterID(masterID);
            if (find == null)
            {
                Users user = new Users();
                user.ID = Guid.NewGuid();
                user.MasterID = masterID;
                user.DeptID = ((BaseInfo)Session["User"]).user.DeptID;
                user.RoleID = new Guid("00000000-0007-0000-0000-000000000000");
                user.RealName = item.CaseName;
                user.IsKeyNode = true;
                user.IsDeleted = false;
                userService.Add(user);
                Master result = masterService.Find((Guid)user.MasterID);
                result.UserID = user.ID;
                masterService.Update(result);
                
            }
        }
        public void makeGiftUser(Master item)
        {
            var masterID = item.ID;
            Users find = userService.FindByMasterID(masterID);
            if (find == null)
            {
                Users user = new Users();
                user.ID = Guid.NewGuid();
                user.MasterID = masterID;
                user.DeptID = ((BaseInfo)Session["User"]).user.DeptID;
                user.RoleID = new Guid("00000000-0007-0000-0000-000000000000");
                user.RealName = item.CaseName;
                user.IsKeyNode = true;
                user.IsDeleted = false;
                userService.Add(user);
                Master result = masterService.Find((Guid)user.MasterID);
                result.UserID = user.ID;
                masterService.Update(result);
            }
        }
        public ActionResult CaseEdit(Guid ID)
        {
            var temp = masterService.Find(ID);
            ViewBag.Master = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult CaseEdit(Master item)
        {
            masterService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult CaseDetails(Guid ID)
        {
            Master caseInfo = masterService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Master = caseInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("CaseEdit");
        }
        [HttpPost]
        public ActionResult CaseDelete(Guid ID)
        {
            masterService.Delete(ID);
            return Content("success");
        }
        public ActionResult GiftCreate()
        {
            Master giftInfo = new Master();
            giftInfo.ID = Guid.NewGuid();
            giftInfo.UserID = ((BaseInfo)Session["User"]).user.ID;
            giftInfo.UnderTakenDept = ((BaseInfo)Session["User"]).user.RealName;
            giftInfo.IsDeleted = false;
            giftInfo.MasterType = "上交";
            ViewBag.Master = giftInfo;
            Session["Flag"] = "Create";
            return View("GiftEdit");
        }
        [HttpPost]
        public ActionResult GiftCreate(Master item)
        {

                //item.ID = Guid.NewGuid();
                masterService.Add(item);
                makeGiftUser(item);
                ModelState.Clear();
                return GiftCreate();

        }
        public ActionResult GiftEdit(Guid ID)
        {
            var temp = masterService.Find(ID);
            ViewBag.Master = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult GiftEdit(Master item)
        {
            masterService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult GiftDetails(Guid ID)
        {
            Master giftInfo = masterService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Master = giftInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("GiftEdit");
        }
        [HttpPost]
        public ActionResult GiftDelete(Guid ID)
        {
            masterService.Delete(ID);
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
            info.QueryString += " and MasterType = '案件'";
            int total = 0;
            var result = masterService.List(info, "`Master`", ref total);

            var data = new
            {
                total = total,
                rows = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCaseGiftList()
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
            var result = masterService.List(info, "`Master`", ref total);

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
            info.QueryString += " and MasterType = '上交'";
            int total = 0;
            var result = masterService.List(info, "`Master`", ref total);
            

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
            info.AuthString = new PermissionService().getPermission();
            return info;
        }
	}
}