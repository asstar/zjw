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
                user.RealName = item.TurnInCode;
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
            foreach (var r in result)
            {
                if (r.MasterType == "上交")
                {
                    r.CaseName = r.TurnInCode;
                    r.CaseFormedDate = r.TurnInDate;
                }
            }

            var data = new
            {
                total = total,
                rows = result
            };

            /*int caseTotal = 0;
            int giftTotal = 0;
            var caseResult = caseService.List(info, "`Master`", ref caseTotal);
            ListModel giftInfo = info.deepClone();
            giftInfo.KeyWord = GiftCaseMapping(giftInfo.KeyWord);
            giftInfo.QueryString = GiftCaseMapping(giftInfo.QueryString);
            giftInfo.Sort = GiftCaseMapping(giftInfo.Sort);
            var giftResult = giftService.List(giftInfo, "Gift", ref giftTotal);
            foreach (var item in giftResult)
            {
                Master copy = new Master();
                copy.ID = item.ID;

                copy.CaseName = item.TurnInCode;
                copy.UnderTakenDept = item.UnderTakenDept;
                copy.TargetName = item.TargetName;
                copy.UnderTaker = item.UnderTaker;
                copy.CaseFormedDate = item.TurnInDate;
                copy.MasterType = item.MasterType;
                caseResult.Add(copy);
            }
            var total = caseResult.Count();

            var data = new
            {
                total = total,
                rows = caseResult
            };*/

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public string GiftCaseMapping(string src)
        {
            string result = null;
            if (src != null)
            {
                Dictionary<string, string> map = new Dictionary<string, string>() { { "CaseName", "TurnInCode" }, { "CaseFormedDate", "TurnInDate" }, };
                foreach (var dict in map)
                {
                    if (result != null)
                    {
                        result = result.Replace(dict.Key, dict.Value);
                    }
                    else
                    {
                        result = src.Replace(dict.Key, dict.Value);
                    }
                    
                }
            }
            return result;
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