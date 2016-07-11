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
        IPropertyService propertyService = new PropertyService();
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
                    ViewBag.Form = formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("DeliverEdit");
                    break;
                case "接收":
                    formInfo.Template = "Sheet.xls";
                    formInfo.SendDept = ((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "委部机关自办案件涉案物品移交清单";
                    batchInfo.BtnTitle = "接收";
                    batchInfo.Action = "/Export/ExportSheet";
                    ViewBag.Form = formInfo;
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
                    ViewBag.Form = formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("DocEdit");
                    break;

                case "出库":
                    formInfo.Template = "Sheet.xls";
                    formInfo.SendDept = "机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案物品调取出库清单";
                    batchInfo.BtnTitle = "出库";
                    batchInfo.Action = "/Export/ExportSheet";
                    ViewBag.Form = formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("SheetEdit");
                    break;
                case "返库":
                    formInfo.Template = "Sheet.xls";
                    formInfo.SendDept = ((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案物品调取入库清单";
                    batchInfo.BtnTitle = "返库";
                    batchInfo.Action = "/Export/ExportSheet";
                    ViewBag.Form = formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("SheetEdit");
                    break;
                    break;
                case "移送审理":
                    formInfo.Template = "Doc.doc";
                    formInfo.SendDept = ((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案款物处置呈批表";
                    batchInfo.BtnTitle = "移送";
                    batchInfo.Action = "/Export/ExportDoc";
                    batchInfo.ReasonTitle = "处置";
                    ViewBag.Form = formInfo;
                    ViewBag.Batch = batchInfo;
                    return View("DeliverEdit");
                    break;
                case "打印处置文书":
                    formInfo.Template = "Doc.doc";
                    formInfo.SendDept = ((BaseInfo)Session["User"]).dept.DeptName;
                    formInfo.ReceiveDept = "机关事务管理局";
                    batchInfo.Title = "中央纪委暂予扣留涉案款物处置呈批表";
                    batchInfo.BtnTitle = "打印处置文书";
                    batchInfo.Action = "/Export/ExportDoc";
                    batchInfo.ReasonTitle = "处置";
                    ViewBag.Form = formInfo;
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
            Property item = new Property();
            Session["Flag"] = "Edit";
            ViewBag.item = item;
            ViewBag.IDs = IDs;
            return View();
        }
        public ActionResult BatchGoodsEdit()
        {
            string IDs = Request["ID"];
            string Type = Request["Type"];
            Property item = new Property();
            Session["Flag"] = "Edit";
            ViewBag.item = item;
            ViewBag.IDs = IDs;
            return View();
        }
        public ActionResult BatchMoneyHandle()
        {
            string IDs = Request["ID"];
            string Type = Request["Type"];
            Property item = new Property();
            Session["Flag"] = "Edit";
            ViewBag.item = item;
            ViewBag.IDs = IDs;
            return View();
        }
        public ActionResult BatchGoodsHandle()
        {
            string IDs = Request["ID"];
            string Type = Request["Type"];
            Property item = new Property();
            Session["Flag"] = "Edit";
            ViewBag.item = item;
            ViewBag.IDs = IDs;
            return View();
        }
        public ActionResult BatchSave(Property item)
        {
            string IDs = Request["IDs"];
            string[] split = IDs.Split(new Char[] { ',' });

            foreach (var i in split)
            {
                Property aData = propertyService.Find(new Guid(i));
                Type itemType = typeof(Property);
                PropertyInfo[] pi = itemType.GetProperties();
                string handleMethod = null;
                foreach (PropertyInfo property in pi)
                {

                    if (property.GetValue(item) != null && property.Name != "ID")
                    {

                        property.SetValue(aData, property.GetValue(item));
                        if (property.Name == "HandleMethod")
                        {
                            handleMethod = (string)property.GetValue(item);
                        }
                    }
                    if (property.Name == "IsFinished")
                    {
                        if (handleMethod != null && handleMethod.IndexOf("拟") == -1)
                        {
                            property.SetValue(aData, "已处置");
                        }
                        else
                        {
                            property.SetValue(aData, "未处置");
                        }
                    }
                }
                propertyService.Update(aData);
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

            List<Property> result = new List<Property>();
            foreach (var i in split)
            {
                InfoLink inst = new InfoLink();
                var prevInfoLink = infoLinkService.FindLastLink(new Guid(i));

                Transfer transfer = new Transfer();
                switch (item.FormType)
                {
                    case "移交":
                        transfer.UserID = ((BaseInfo)Session["User"]).user.ID;
                        transfer.CurrentStatus = "移交";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        transfer.UserID = new Guid("00000000-0003-0000-0000-000000000000");
                        transfer.CurrentStatus = "接收";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        break;
                    case "接收":
                        transfer.UserID = ((BaseInfo)Session["User"]).user.ID;
                        transfer.CurrentStatus = "保管";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        break;
                    case "调取":
                        transfer.UserID = ((BaseInfo)Session["User"]).user.ID;
                        transfer.CurrentStatus = "调取";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        transfer.UserID = new Guid("00000000-0003-0000-0000-000000000000");
                        transfer.CurrentStatus = "待出库";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        break;
                    case "出库":
                        transfer.UserID = ((BaseInfo)Session["User"]).user.ID;
                        transfer.CurrentStatus = "出库";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        break;
                    case "返库":
                        transfer.UserID = (Guid)infoLinkService.FindLastNodeByStatus(prevInfoLink,"调取").UserID;
                        transfer.CurrentStatus = "归还";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        transfer.UserID = ((BaseInfo)Session["User"]).user.ID;
                        transfer.CurrentStatus = "保管";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                        break;
                    case "移送审理":
                        transfer.UserID = new Guid("00000000-0005-0000-0000-000000000000");
                        transfer.CurrentStatus = "移送审理";
                        infoLinkService.MakeNextNode(prevInfoLink, transfer);
                         var data = propertyService.Find((Guid)prevInfoLink.PropertyID);
                        data.IsDelivered = true;
                        propertyService.Update(data);
                        break;
                }
                infoLinkService.SetActive(prevInfoLink.ID);

            }
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");
        }
        public JsonResult GetList()
        {
            string IDs = Request["IDs"];
            string[] split = IDs.Split(new Char[] { ',' });

            int total = 0;
            List<Property> result = new List<Property>();
            foreach (var item in split)
            {
                result.Add(propertyService.Find(new Guid(item)));
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