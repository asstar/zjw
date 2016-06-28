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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MoneyCreate()
        {
            Money moneyInfo = new Money();
            ViewBag.Money = moneyInfo;
            Session["Flag"] = "Create";
            return View("MoneyEdit");
        }
        [HttpPost]
        public ActionResult MoneyCreate(Money item)
        {

            try
            {
                item.ID = Guid.NewGuid();
                moneyService.Add(item);
                return MoneyCreate();
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }
        public ActionResult GoodsCreate()
        {
            Goods goodsInfo = new Goods();
            ViewBag.Goods = goodsInfo;
            Session["Flag"] = "Create";
            return View("GoodsEdit");
        }
	}
}