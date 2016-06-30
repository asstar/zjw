using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Model;
using IBLL;
using BLL;
namespace zjw.Controllers
{
    public class FileController : Controller
    {
        IAffixService affixService = new AffixService();
        //
        // GET: /FileUpload/
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData, string guid, string folder)
        {
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/UploadFiles/");
                    //DirectoryUtil.AssertDirExist(filePath);

                    string fileName = Path.GetFileName(fileData.FileName);      //原始文件名称
                    string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                    string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称

                    if (!System.IO.Directory.Exists(filePath))
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    fileData.SaveAs(Path.Combine(filePath, saveName));


                    Affix affix = new Affix();
                    affix.ID = Guid.NewGuid();
                    affix.MasterID = new Guid(Request.Form["ID"]);
                    affix.FileName = fileName;
                    affix.SaveName = saveName;
                    affix.FileExtension = fileExtension;
                    affix.ContentType = fileData.ContentType;
                    System.IO.Stream stream = fileData.InputStream;
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);
                    stream.Close();
                    affix.Content = buffer;
                    affixService.AddMore(affix);
                    return Content("true");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return Content("false");
                }
            }
            else
            {
                return Content("false");
            }
        }
        public JsonResult GetList(Guid ID)
        {

            using (DBEntities db = new DBEntities())
            {
                int total = 0;
                //string dept = (string)Session["Dept"];
                IEnumerable<Affix> temp = null; ;
                string sql = "select * FROM Affix where MasterID='" + ID+"' and IsDeleted=0";
                temp = db.Database.SqlQuery<Affix>(sql);
                var processList=temp.ToList<Affix>();
                foreach (var item in processList)
                {
                    item.Content=null;
                }
                total = temp.Count();
                var data = new
                {
                    total = total,
                    rows = processList 
                };


                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult Download(string ID)
        {
            Guid guid = new Guid(ID);

            var affix = affixService.Find(guid);

            return File(affix.Content, " ",/*affix.ContentType*/ affix.FileName);
        }
        public ActionResult Delete(string ID)
        {

            Affix affix = new Affix();

            Guid guid = new Guid(ID);

            affix = affixService.Find(guid);

            affix.IsDeleted = true;

            affixService.Update(affix);

            return Content("true");
        }
    }
}