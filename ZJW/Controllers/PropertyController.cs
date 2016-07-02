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

        public JsonResult GetGoodsLastData()
        {
            string sql = "select * FROM Goods Order by TimeStamp limit 1";
            var result = goodsService.SqlQuery(sql);
            Goods max = result.FirstOrDefault();
            return Json(max, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMoneyLastData()
        {
            string sql = "select * FROM Money Order by TimeStamp limit 1";
            var result = moneyService.SqlQuery(sql);
            Money max = result.FirstOrDefault();
            return Json(max, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FindCase()
        {
            Session["UsePrev"] = false;
            return View();
        }
        public ActionResult MoneyCreate()
        {
            Money moneyInfo = new Money();
            moneyInfo.ID = Guid.NewGuid();
            ViewBag.Money = moneyInfo;
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
            ViewBag.Goods = goodsInfo;
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
            var result = moneyService.List(info, "`Money`", ref total);
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
            var result = goodsService.List(info, "`Goods`", ref total);
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