using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspose.Words;
using Aspose.Cells;
using Model;
using IBLL;
using BLL;
using System.Data;
using System.Collections;
using System.Reflection;
namespace zjw.Controllers
{
    public class ExportController : Controller
    {
        IGoodsService goodsService = new GoodsService();
        IFormService formService = new FormService();
        public ActionResult ExportDoc(Form item,Batch batch)
        {
            /*var formInfo = formService.Find(item.ID);
            if (formInfo == null)
            {
                formService.Add(item);
            }
            else
            {
                formService.Update(item);
            }*/
            Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("/") + "Template/Word.doc");
            Aspose.Words.DocumentBuilder builder = new Aspose.Words.DocumentBuilder(doc);

            string IDs = item.Data;
            string[] split = IDs.Split(new Char[] { ',' });

            int total = 0;
            List<Goods> result = new List<Goods>();
            foreach (var i in split)
            {
                result.Add(goodsService.Find(new Guid(i)));
            }
            List<Form> forms = new List<Form>();
            forms.Add(item);
            List<Batch> batchs = new List<Batch>();
            batchs.Add(batch);
            var dt = ListToDataTable(result);
            var formDT = ListToDataTable(forms);
            var batchDT = ListToDataTable(batchs);
            dt.TableName = "Goods";
            formDT.TableName = "Form";
            doc.MailMerge.Execute(batchDT);
            doc.MailMerge.Execute(formDT);
            doc.MailMerge.ExecuteWithRegions(dt);
            string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".doc";
            doc.Save(Server.MapPath("/doc/") + filename);
            return File(Server.MapPath("/doc/" + filename), "application/msword", Url.Encode(filename));
        }
        //
        // GET: /Export/
        public ActionResult ExportSheet(Form item, Batch batch)
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
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Open(Server.MapPath("/") + "Template/Sheet.xls");
            designer.SetDataSource("Title", batch.Title);
            designer.SetDataSource("SendDept", item.SendDept);
            designer.SetDataSource("SendLeader", item.SendLeader);
            designer.SetDataSource("Sender", item.Sender);
            if (item.SendDate != null)
            {
                string SendDate = ((DateTime)item.SendDate).ToString("yyyy年MM月dd日");
                designer.SetDataSource("SendDate", SendDate);
            }
            else
            {
                designer.SetDataSource("SendDate", "");
            }
            
            designer.SetDataSource("ReceiveDept", item.ReceiveDept);
            designer.SetDataSource("ReceiveLeader", item.ReceiveLeader);
            designer.SetDataSource("Receiver", item.Receiver);
            if (item.SendDate != null)
            {
                string ReceiveDate = ((DateTime)item.ReceiveDate).ToString("yyyy年MM月dd日");
                designer.SetDataSource("ReceiveDate", ReceiveDate);
            }
            else
            {
                designer.SetDataSource("ReceiveDate", "");
            }

            string filename = DateTime.Now.ToString("yyyyMMddhhmmss") + ".xls";

            string IDs = item.Data;
            string[] split = IDs.Split(new Char[] { ',' });

            int total = 0;
            List<Goods> result = new List<Goods>();
            foreach (var i in split)
            {
                result.Add(goodsService.Find(new Guid(i)));
            }

            int sum=0;
            foreach (var j in result)
            {
                sum += int.Parse(j.PackageNumber);
            }
            designer.SetDataSource("Sum", sum+"件");
            var dt = ListToDataTable(result);
            dt.TableName = "Goods";
            designer.SetDataSource(dt);
            total = result.Count();
            designer.Process();

            designer.Save(Server.MapPath("/xls/") + filename);
            return File(Server.MapPath("/xls/" + filename), "application/msexcel", Url.Encode(filename));
        }
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
	}

}