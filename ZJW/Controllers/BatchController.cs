using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using IBLL;
using BLL;
using System.Reflection;
namespace zjw.Controllers
{
    public class BatchController : BaseController
    {
        IGoodsService goodsService = new GoodsService();
        IInfoLinkService infoLinkService = new InfoLinkService();
        //
        // GET: /Batch/
        IFormService formService = new FormService();

        public ActionResult Create()
        {
            string IDs = Request["ID"];
            string Type = Request["Type"];
            Form formInfo = new Form();
            Batch batchInfo = new Batch();
            Transfer transfer = new Transfer();
            
            formInfo.ID = Guid.NewGuid();
            formInfo.UserID = ((BaseInfo)Session["User"]).user.ID;
            formInfo.FormType = Type;
            formInfo.Data = IDs;
            

            formInfo.IsDeleted = false;
            Session["Flag"] = "Create";
            switch (Type)
            {
                case "移交":
                    formInfo.Template = "Sheet.xls";
                    formInfo.SendDept = ((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "委部机关自办案件涉案物品移交清单";
                    batchInfo.BtnTitle = "移交";
                    batchInfo.Action = "/Export/ExportSheet";
                    transfer.CurrentStatus = "管理局保管";
                    transfer.PrevStatus = "移交管理局";
                    transfer.UserID = new Guid("00000000-0003-0000-0000-000000000000");
                    Session["Transfer"] = transfer;
                    ViewBag.Form= formInfo;
                    ViewBag.Batch = batchInfo;

                    return View("SheetEdit");
                    break;
                case "调取": 
                    formInfo.Template = "Doc.doc";
                    formInfo.SendDept = ((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案物品调取呈批表";
                    batchInfo.BtnTitle = "调取";
                    batchInfo.Action = "/Export/ExportDoc";
                    batchInfo.ReasonTitle = "调取";

                    transfer.CurrentStatus = "待调取";
                    transfer.PrevStatus = "待出库";
                    transfer.UserID = ((BaseInfo)Session["User"]).user.ID;
                    Session["Transfer"] = transfer;
                    ViewBag.Form= formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("DocEdit");
                    break;

                case "出库": 
                    formInfo.Template = "Sheet.xls";
                    formInfo.SendDept ="机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案物品调取出库清单";
                    batchInfo.BtnTitle = "出库";
                    batchInfo.Action = "/Export/ExportSheet";

                    transfer.CurrentStatus = "调取";
                    transfer.PrevStatus = "出库";
                    Session["Transfer"] = transfer;
                    ViewBag.Form= formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("SheetEdit");
                    break;
                case "返库": 
                    formInfo.Template = "Sheet.xls";
                    formInfo.SendDept =((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案物品调取入库清单";
                    batchInfo.BtnTitle = "返库";
                    batchInfo.Action = "/Export/ExportSheet";

                    transfer.CurrentStatus = "管理局保管";
                    transfer.PrevStatus = "返库";
                    transfer.UserID = ((BaseInfo)Session["User"]).user.ID;
                    Session["Transfer"] = transfer;
                    ViewBag.Form= formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("SheetEdit");
                    break;
                    break;
                case "提交审理": 
                    formInfo.Template = "Doc.doc";
                    formInfo.SendDept = ((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案款物处置呈批表";
                    batchInfo.BtnTitle="处置";
                    batchInfo.Action = "/Export/ExportDoc";
                    batchInfo.ReasonTitle = "处置";

                    transfer.CurrentStatus = "提交审理";
                    transfer.PrevStatus = null;
                    transfer.UserID = new Guid("00000000-0005-0000-0000-000000000000");
                    Session["Transfer"] = transfer;
                    ViewBag.Form= formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("DocEdit");
                    break;
            }
            return View();
        }
        public ActionResult BatchMoneyEdit()
        {
            string IDs = Request["ID"];
            string Type = Request["Type"];
            return View();
        }
        public ActionResult BatchGoodsEdit()
        {
            string IDs = Request["ID"];
            string Type = Request["Type"];
            Goods item = new Goods();
            ViewBag.item = item;
            ViewBag.IDs = IDs;
            return View();
        }
        public ActionResult BatchGoodsSave(Goods item)
        {
            string IDs = Request["IDs"];
            string[] split = IDs.Split(new Char[] { ',' });

            foreach (var i in split)
            {
                Goods aData = goodsService.Find(new Guid(i));
                Type itemType = typeof(Goods);
                PropertyInfo[] pi = itemType.GetProperties();
                string handleMethod = null;
                foreach (PropertyInfo property in pi)
                {
                    
                    if (property.GetValue(item) != null&&property.Name!="ID")
                    {

                        property.SetValue(aData, property.GetValue(item));
                        if(property.Name=="HandleMethod"){
                            handleMethod = (string)property.GetValue(item);
                        }
                    }
                    if (property.Name == "IsFinished")
                    {
                        if (handleMethod != null && handleMethod.IndexOf("拟") == -1)
                        {
                            property.SetValue(aData, "已处置");
                        }
                    }
                }
                goodsService.Update(aData);
            }
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");
        }
        public ActionResult Edit(Guid ID)
        {
            var formInfo = formService.Find(ID);
            ViewBag.Form = formInfo;
            Session["Flag"] = "Edit";
            Session["UsePrev"] = true;
            return View("SheetEdit");
        }
        public ActionResult Save(Form item)
        {
            var formInfo = formService.Find(item.ID);
            if (formInfo == null)
            {
                formService.Add(item);
            }
            else
            {
                formService.Update(item);
            }
            
            return Content("<script type=\"text/javascript\">history.go(-1);</script>");
        }
        public ActionResult Transfer(Form item)
        {
            var formInfo = formService.Find(item.ID);
            if (formInfo == null)
            {
                formService.Add(item);
            }
            else
            {
                formService.Update(item);
            }
            string IDs = item.Data;
            string[] split = IDs.Split(new Char[] { ',' });

            List<Goods> result = new List<Goods>();
            foreach (var i in split)
            {
                InfoLink inst = new InfoLink();
                var prevInfoLink = infoLinkService.FindLastTransferLink(new Guid(i));
                Transfer check=(Transfer)Session["Transfer"];
                if (item.FormType == "出库")
                {
                    infoLinkService.MakeOutLink(prevInfoLink, (Transfer)Session["Transfer"]);
                }
                else
                {
                    inst = infoLinkService.MakeLink(prevInfoLink, (Transfer)Session["Transfer"]);
                    //inst = prevInfoLink.MakeLink(inst, (Transfer)Session["Transfer"]);
                    infoLinkService.SetActive(inst.ID);
                }
                if (item.FormType == "提交审理")
                {
                    var data = goodsService.Find((Guid)prevInfoLink.PropertyID);
                    data.IsDelivered = true;
                    goodsService.Update(data);
                }

            }
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");
        }
        public JsonResult GetList()
        {
            string IDs = Request["IDs"];
            string[] split = IDs.Split(new Char[] { ',' });

            int total = 0;
            List<Goods> result = new List<Goods>();
            foreach (var item in split)
            {
                result.Add(goodsService.Find(new Guid(item)));
            }
            total = result.Count();

            var data = new
            {
                total = total,
                rows = result
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}