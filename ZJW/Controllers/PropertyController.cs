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
    public class PropertyController : BaseController
    {
        //
        // GET: /Property/
        IMasterService masterService = new MasterService();
        IPropertyService propertyService = new PropertyService();
        IPropertyViewService propertyViewService = new PropertyViewService();

        IInfoLinkService infoLinkService = new InfoLinkService();
        public JsonResult GetGoodsLastData()
        {
            string sql = "select * FROM PropertyView where UserID='" + ((BaseInfo)Session["User"]).user.ID + "' and IsDeleted=0 and PropertyFlag = '物品' Order by TimeStamp desc limit 1";
            var result = propertyViewService.SqlQuery(sql);
            PropertyView max = result.FirstOrDefault();
            return Json(max, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMoneyLastData()
        {
            string sql = "select * FROM PropertyView where UserID='" + ((BaseInfo)Session["User"]).user.ID + "'and IsDeleted=0 and PropertyFlag = '款项' Order by TimeStamp desc limit 1";
            var result = propertyViewService.SqlQuery(sql);
            PropertyView max = result.FirstOrDefault();
            return Json(max, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FindCase()
        {
            Session["UsePrev"] = false;
            return View();
        }

        public Object setMasterInfo(Object item)
        {

            if (((BaseInfo)Session["User"]).role.RoleName == "专案组")
            {
                if (item.GetType() == typeof(Property))
                {
                    var money = (Property)item;
                    money.MasterType = "案件";
                    money.MasterID = ((BaseInfo)Session["User"]).user.MasterID;
                    money.CaseName = masterService.Find((Guid)(money.MasterID)).CaseName;
                    money.CaseCode = masterService.Find((Guid)(money.MasterID)).CaseCode;

                    return money;

                }
                if (item.GetType() == typeof(Property))
                {
                    var goods = (Property)item;
                    goods.MasterType = "案件";
                    goods.MasterID = ((BaseInfo)Session["User"]).user.MasterID;
                    goods.CaseName = masterService.Find((Guid)(goods.MasterID)).CaseName;
                    goods.CaseCode = masterService.Find((Guid)(goods.MasterID)).CaseCode;
                    return goods;
                }
                return item;
            }
            else
            {
                return item;
            }

        }
        public ActionResult MoneyCreate()
        {
            Property moneyInfo = new Property();
            moneyInfo.ID = Guid.NewGuid();
            moneyInfo.IsDeleted = false;
            moneyInfo.PropertyFlag = "款项";
            ViewBag.Property = setMasterInfo(moneyInfo);
            Session["Flag"] = "Create";
            return View("MoneyEdit");
        }
        [HttpPost]
        public ActionResult MoneyCreate(Property item)
        {
            if (item.HandleMethod != null)
            {
                if (item.HandleMethod.IndexOf("拟") == -1)
                {
                    item.IsFinished = "已处置";
                }
                else
                {
                    item.IsFinished = "未处置";
                }
            }
            else
            {
                item.IsFinished = "未处置";
            }
            propertyService.Add(item);
            infoLinkService.Add(item.ID, "款项");
            ModelState.Clear();
            return MoneyCreate();

        }
        public ActionResult MoneyEdit(Guid ID)
        {
            var temp = propertyService.Find(ID);
            ViewBag.Property = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult MoneyEdit(Property item)
        {
            if (item.HandleMethod != null)
            {
                if (item.HandleMethod.IndexOf("拟") == -1)
                {
                    item.IsFinished = "已处置";
                }
                else
                {
                    item.IsFinished = "未处置";
                }
            }
            else
            {
                item.IsFinished = "未处置";
            }
            propertyService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult MoneyDetails(Guid ID)
        {
            Property moneyInfo = propertyService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Property = moneyInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("MoneyEdit");
        }
        [HttpPost]
        public ActionResult MoneyDelete(Guid ID)
        {
            propertyService.Delete(ID);
            return Content("success");
        }
        public ActionResult GoodsCreate()
        {
            Property goodsInfo = new Property();
            goodsInfo.ID = Guid.NewGuid();
            goodsInfo.IsDeleted = false;
            goodsInfo.PropertyFlag = "物品";
            ViewBag.Property = setMasterInfo(goodsInfo);
            Session["Flag"] = "Create";
            return View("GoodsEdit");
        }
        [HttpPost]
        public ActionResult GoodsCreate(Property item)
        {

            if (item.HandleMethod != null)
            {
                if (item.HandleMethod.IndexOf("拟") == -1)
                {
                    item.IsFinished = "已处置";
                }
                else
                {
                    item.IsFinished = "未处置";
                }
            }
            else
            {
                item.IsFinished = "未处置";
            }
            //item.ID = Guid.NewGuid();
            propertyService.Add(item);
            infoLinkService.Add(item.ID, "物品");
            ModelState.Clear();
            return GoodsCreate();

        }
        public ActionResult GoodsEdit(Guid ID)
        {
            var temp = propertyService.Find(ID);
            ViewBag.Property = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult GoodsEdit(Property item)
        {
            if (item.HandleMethod != null)
            {
                if (item.HandleMethod.IndexOf("拟") == -1)
                {
                    item.IsFinished = "已处置";
                }
                else
                {
                    item.IsFinished = "未处置";
                }
            }
            else
            {
                item.IsFinished = "未处置";
            }
            propertyService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult GoodsDetails(Guid ID)
        {
            Property goodsInfo = propertyService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Property = goodsInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("GoodsEdit");
        }
        [HttpPost]
        public ActionResult GoodsDelete(Guid ID)
        {
            propertyService.Delete(ID);
            return Content("success");
        }
        public ActionResult MoneyList()
        {
            Session["UsePrev"] = false;
            return View();
        }
        public JsonResult GetMoneyList()
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
            info.QueryString += " and PropertyFlag = '款项'";
            var result = propertyViewService.List(info, "`PropertyView`", ref total);
            

            var data = new
            {
                total = total,
                rows = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GoodsList()
        {
            Session["UsePrev"] = false;
            string type = Request["Type"];
            BtnModel btn = new BtnModel();
            if (type != null)
            {
                btn.type = type;
                switch (type)
                {
                    case "Transfer":
                        //btn.setArray(true, true, true, true, true);
                        btn.isTransferable = true;
                        break;
                    case "Out":
                        btn.isOutable = true;
                        break;
                    case "Return":
                        btn.isReturnable = true;
                        break;
                    case "Handle":
                        btn.isHandleable = true;
                        break;
                    case "Deliver":
                        btn.isDeliverable = true;
                        break;
                    case "Borrow":
                        btn.isBorrowable = true;
                        break;
                    case "Edit":
                        btn.isEditable = true;
                        break;
                }
            }
            Session["BtnModel"] = btn;
            ViewBag.Btn = btn;
            return View();
        }
        public JsonResult GetGoodsList()
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

            BtnModel btnModel = (BtnModel)Session["BtnModel"];
            string type = btnModel.type;
            List<PropertyView> result = new List<PropertyView>();
            if (type != null)
            {
                switch (type)
                {
                    case "Transfer":
                        info.QueryString += " and Status='暂扣' and Active=1";
                        break;
                    case "Out":
                        info.QueryString += " and Status='待出库' and Active=1";
                        break;
                    case "Return":
                        info.QueryString += " and Status='出库' and Active=1 ";
                        break;
                    case "Handle":
                        info.QueryString += " and IsFinished<>'已处置' and Active=1";
                        break;
                    case "Deliver":
                        info.QueryString += " and IsFinished<>'已处置' and IsDelivered<>1 and Active=1";
                        break;
                    case "Borrow":
                        info.QueryString += " and (Status='移交管理局' OR Status='返库') and Active=1";
                        break;
                }
            }
            info.QueryString += " and PropertyFlag = '物品'";
            result = propertyViewService.List(info, "`PropertyView`", ref total);

            Dictionary<Guid, PropertyView> dict = new Dictionary<Guid, PropertyView>();
            List<PropertyView> process = new List<PropertyView>();
            foreach (var i in result)
            {
                PropertyView trace = i;
                while (trace.Next != null)
                {
                    trace = propertyViewService.FindLinkID((Guid)trace.Next);
                }
                if (!dict.ContainsKey(trace.LinkID))
                {
                    dict.Add(trace.LinkID, trace);
                    process.Add(trace);
                }
            }
            result = process;
           

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