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
        IMasterService masterService = new MasterService();
        IFormViewService formViewService = new FormViewService();
        //
        // GET: /Batch/
        IFormService formService = new FormService();

        public ActionResult Create()
        {
            string IDs = Request["ID"];
            string Type = Request["Type"];
            string propertyName = Request["PropertyName"];
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
                    if (propertyName == "款项")
                    {
                        return View("DeliverMoneyEdit");
                    }
                    else
                    {
                        return View("DeliverGoodsEdit");
                    }
                    break;
                case "接收":
                    formInfo.Template = "Sheet.xls";
                    formInfo.SendDept = null;
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
                    if (propertyName == "款项")
                    {
                        return View("DeliverMoneyEdit");
                    }
                    else
                    {
                        return View("DeliverGoodsEdit");
                    }
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
            if (formInfo.FormType == "接收" || formInfo.FormType == "入库" || formInfo.FormType == "出库")
            {
                Batch batchInfo = new Batch();
                batchInfo.Title = formInfo.FormName;
                batchInfo.BtnTitle = formInfo.FormType;
                batchInfo.Action = "/Export/ExportSheet";
                ViewBag.Batch = batchInfo;
                return View("SheetEdit");
            }
            else
            {
                Batch batchInfo = new Batch();
                batchInfo.Title = formInfo.FormName;
                batchInfo.BtnTitle = formInfo.FormType;
                batchInfo.Action = "/Export/ExportDoc";
                ViewBag.Batch = batchInfo;
                return View("DocEdit");
            }
        }
        public ActionResult Detail(Guid ID)
        {
            var formInfo = formService.Find(ID);
            ViewBag.Form = formInfo;
            Session["Flag"] = "Detail";
            Session["UsePrev"] = true;
            if (formInfo.FormType == "接收" || formInfo.FormType == "入库" || formInfo.FormType == "出库")
            {
                Batch batchInfo = new Batch();
                batchInfo.Title = formInfo.FormName;
                batchInfo.BtnTitle = formInfo.FormType;
                batchInfo.Action = "/Export/ExportSheet";
                ViewBag.Batch = batchInfo;
                return View("SheetEdit");
            }
            else
            {
                Batch batchInfo = new Batch();
                batchInfo.Title = formInfo.FormName;
                batchInfo.BtnTitle = formInfo.FormType;
                batchInfo.Action = "/Export/ExportDoc";
                ViewBag.Batch = batchInfo;
                return View("DocEdit");
            }
        }
        public ActionResult Save(Form item)
        {
            string IDs = item.Data;
            string[] split = IDs.Split(new Char[] { ',' });
            Guid firstItem = new Guid(split[0]);
            Property temp = propertyService.Find(firstItem);
            item.MasterID = temp.MasterID;
            if (FormDict.ContainsKey(item.FormType))
            {
                item.FormName = FormDict[item.FormType];
            }
            var formInfo = formService.Find(item.ID);
            if (formInfo == null)
            {
                formService.Add(item);
            }
            else
            {
                formService.Update(item);
            }

            return Content("<script type=\"text/javascript\">history.go(-2);</script>");
        }
        Dictionary<string, string> FormDict = new Dictionary<string, string>() { { "接收", "委部机关自办案件涉案物品移交清单" }, { "出库", "中央纪委暂予扣留涉案物品调取出库清单" }, { "返库", "中央纪委暂予扣留涉案物品调取入库清单" }, { "调取", "中央纪委暂予扣留涉案物品调取呈批表" }, { "打印处置文书", "中央纪委暂予扣留涉案款物处置呈批表" } };
        public ActionResult Transfer(Form item)
        {
            string IDs = item.Data;
            string[] split = IDs.Split(new Char[] { ',' });
            Guid firstItem = new Guid(split[0]);
            Property temp = propertyService.Find(firstItem);
            item.MasterID = temp.MasterID;
            if (FormDict.ContainsKey(item.FormType))
            {
                item.FormName = FormDict[item.FormType];
            }
            
            var formInfo = formService.Find(item.ID);
            if (formInfo == null)
            {
                formService.Add(item);
            }
            else
            {
                formService.Update(item);
            }


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
        public ActionResult ReportList()
        {
            return View();
        }

        public JsonResult GetReportList()
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

            info.QueryString += " And FormName is not null ";
            int total = 0;
            var result = formViewService.List(info, "`FormView`", ref total);

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