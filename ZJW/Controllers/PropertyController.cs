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
        IMoneyService moneyService = new MoneyService();
        IGoodsService goodsService = new GoodsService();
        IMoneyViewService moneyViewService = new MoneyViewService();
        IGoodsViewService goodsViewService = new GoodsViewService();
        ICaseService caseService = new CaseService();
        IGiftService giftService = new GiftService();
        IInfoLinkService infoLinkService = new InfoLinkService();
        public JsonResult GetGoodsLastData()
        {
            string sql = "select * FROM GoodsView where UserID='" + ((BaseInfo)Session["User"]).user.ID + "' and IsDeleted=0 Order by TimeStamp desc limit 1";
            var result = goodsViewService.SqlQuery(sql);
            GoodsView max = result.FirstOrDefault();
            return Json(max, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMoneyLastData()
        {
            string sql = "select * FROM MoneyView where UserID='" + ((BaseInfo)Session["User"]).user.ID + "'and IsDeleted=0  Order by TimeStamp desc limit 1";
            var result = moneyViewService.SqlQuery(sql);
            MoneyView max = result.FirstOrDefault();
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
                if (item.GetType() == typeof(Money))
                {
                    var money = (Money)item;
                    money.MasterType="款项";
                    money.MasterID=((BaseInfo)Session["User"]).user.MasterID;
                    money.CaseName=caseService.Find((Guid)(money.MasterID)).CaseName;
                    money.CaseCode = caseService.Find((Guid)(money.MasterID)).CaseCode;
                    
                    return money;
                    
                }
                if (item.GetType() == typeof(Goods))
                {
                    var goods = (Goods)item;
                    goods.MasterType = "款项";
                    goods.MasterID = ((BaseInfo)Session["User"]).user.MasterID;
                    goods.CaseName = caseService.Find((Guid)(goods.MasterID)).CaseName;
                    goods.CaseCode = caseService.Find((Guid)(goods.MasterID)).CaseCode;
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
            Money moneyInfo = new Money();
            moneyInfo.ID = Guid.NewGuid();
            moneyInfo.IsDeleted = false;
            ViewBag.Money = setMasterInfo(moneyInfo);
            Session["Flag"] = "Create";
            return View("MoneyEdit");
        }
        [HttpPost]
        public ActionResult MoneyCreate(Money item)
        {

            try
            {
                //item.ID = Guid.NewGuid();
                moneyService.Add(item);
                infoLinkService.Add(item.ID, "款项");
                ModelState.Clear();
                return MoneyCreate();
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }
        public ActionResult MoneyEdit(Guid ID)
        {
            var temp = moneyService.Find(ID);
            ViewBag.Money = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult MoneyEdit(Money item)
        {
            moneyService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult MoneyDetails(Guid ID)
        {
            Money moneyInfo = moneyService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Money = moneyInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("MoneyEdit");
        }
        [HttpPost]
        public ActionResult MoneyDelete(Guid ID)
        {
            moneyService.Delete(ID);
            return Content("success");
        }
        public ActionResult GoodsCreate()
        {
            Goods goodsInfo = new Goods();
            goodsInfo.ID = Guid.NewGuid();
            goodsInfo.IsDeleted = false;
            ViewBag.Goods = setMasterInfo(goodsInfo);
            Session["Flag"] = "Create";
            return View("GoodsEdit");
        }
        [HttpPost]
        public ActionResult GoodsCreate(Goods item)
        {

            try
            {
                //item.ID = Guid.NewGuid();
                goodsService.Add(item);
                infoLinkService.Add(item.ID, "物品");
                ModelState.Clear();
                return GoodsCreate();
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }
        public ActionResult GoodsEdit(Guid ID)
        {
            var temp = goodsService.Find(ID);
            ViewBag.Goods = temp;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View();
        }
        [HttpPost]
        public ActionResult GoodsEdit(Goods item)
        {
            goodsService.Update(item);
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");

        }
        public ActionResult GoodsDetails(Guid ID)
        {
            Goods goodsInfo = goodsService.Find(ID); //db.ClueInfo.Find(ID);
            ViewBag.Goods = goodsInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            return View("GoodsEdit");
        }
        [HttpPost]
        public ActionResult GoodsDelete(Guid ID)
        {
            moneyService.Delete(ID);
            return Content("success");
        }
        public ActionResult MoneyList()
        {
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
            var result = moneyViewService.List(info, "`MoneyView`", ref total);
            total = result.Count();

            var data = new
            {
                total = total,
                rows = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GoodsList()
        {
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
            var result = goodsViewService.List(info, "`GoodsView`", ref total);
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
            info.AuthString = new PermissionService().getPermission();
            return info;
        }
	}
}