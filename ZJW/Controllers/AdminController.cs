﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using IBLL;
using BLL;
using System.Diagnostics;
namespace zjw.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/
        //XSEntities db = new XSEntities();
        IUserService userService = new UserService();
        IDeptService deptService = new DeptService();
        IRoleService roleService = new RoleService();
        IUserDeptService userDeptService = new UserDeptService();
        IRoleMenuService RoleMenuService = new RoleMenuService();
        IMasterService masterService = new MasterService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogList()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            ViewBag.users = ((BaseInfo)Session["User"]).user;
            return View();
        }

        [HttpPost]
        public ActionResult SavePassword(Users item)
        {
            try
            {
                // TODO: Add update logic here
                item.IsDeleted = false;
                userService.Update(item);
                Debug.WriteLine("Save Password OK");
                ViewBag.users = ((BaseInfo)Session["User"]).user;
                return View("ChangePassword");
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }
        }


        public ActionResult UsersList()
        {
            return View();
        }
        public ActionResult UsersEdit()
        {
            string type = Request["type"];
            Session["UserEditType"] = type;
            Guid id = new Guid();
            if (Request["type"] != "Create")
                id = new Guid(Request["ID"]);
            if (type == "Create")
            {
                Users user = new Users();
                user.ID = Guid.NewGuid();
                ViewBag.user = user;
                return View();
            }
            else
            {
                Users user = userService.Find(id);
                ViewBag.user = user;
                return View();
            }
        }

        public ActionResult CaseUserEdit()
        {
            var masterID = new Guid(Request["MasterID"]);
            Users find = userService.FindByMasterID(masterID);
            if(find==null)
            {
                Session["UserEditType"] = "Create";
                Users user = new Users();
                user.ID = Guid.NewGuid();
                user.MasterID = masterID;
                user.DeptID = ((BaseInfo)Session["User"]).user.DeptID;
                user.RoleID = new Guid("00000000-0007-0000-0000-000000000000"); ;
                user.IsKeyNode = true;
                ViewBag.user = user;
                ViewBag.baseInfo = ((BaseInfo)Session["User"]);
                return View();
            }
            else
            {
                Session["UserEditType"] = "Edit";
                Users user = userService.Find(find.ID);
                ViewBag.user = user;
                return View();
            }
        }
        public ActionResult CaseUserSaveOrUpdate(Users item)
        {

            string editType = (string)Session["UserEditType"];
            Debug.WriteLine(editType);
            if (editType == "Create")
            {
                Guid guid = Guid.NewGuid();
                item.ID = guid;
                item.RoleID = item.RoleID;
                item.IsDeleted = false;
                userService.Add(item);
                Master result = masterService.Find((Guid)item.MasterID);
                result.UserID = item.ID;
                masterService.Update(result);
                return Content("<script type=\"text/javascript\">history.go(-2);</script>");
            }
            if (editType == "Edit")
            {
                item.IsDeleted = false;
                userService.Update(item);
                Master result = masterService.Find((Guid)item.MasterID);
                result.UserID = item.ID;
                masterService.Update(result);
                return Content("<script type=\"text/javascript\">history.go(-2);</script>");
            }
            return Content("<script type=\"text/javascript\">history.go(-2);</script>");
        }
        
        [HttpPost]
        public ActionResult UsersDelete(Guid ID)
        {
            userService.Delete(ID);
            return Content("true");
        }
        public ActionResult CheckName()
        {
            string userName = Request["UserName"];

            Users result = userService.FindByUserName(userName);
            if (result != null)
            {
                return Content("false");
            }
            return Content("true");
        }

        public ActionResult SaveOrUpdate(Users item)
        {

            string editType = (string)Session["UserEditType"];
            Debug.WriteLine(editType);
            string deptTree = Request["deptCombTree"];
            if (deptTree != null)
            {
                userDeptService.DeleteByUserID(item.ID);
                string[] array = deptTree.Split(',');
                foreach (string s in array)
                {
                    UserDept obj = new UserDept();
                    obj.ID = Guid.NewGuid();
                    obj.UserID = item.ID;
                    obj.DeptID = new Guid(s);
                    userDeptService.Add(obj);

                }
            }
            if (editType == "Create")
            {
                Guid guid = Guid.NewGuid();
                item.ID = guid;
                item.RoleID = item.RoleID;
                item.IsDeleted = false;
                userService.Add(item);

                return Content("true");
            }
            if (editType == "Edit")
            {
                item.RoleID = item.RoleID;
                item.IsDeleted = false;
                userService.Update(item);
                return Content("true");
            }
            return Content("false");
        }

        public JsonResult GetBindUserList()
        {
            IEnumerable<Users> temp = null;
            string sql = null;

            sql = "SELECT * FROM Users where IsKeyNode=1 and IsDeleted=0";


            temp = userService.SqlQuery(sql);

            return Json(temp.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsersList()
        {
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 30 : int.Parse(Request["rows"]);
            string sort = Request["sort"];
            string direction = Request["order"];
            string uName = Request["RealName"];
            string sqlUserName = "Where  IsDeleted=0";
            if (uName != null)
            {
                sqlUserName = " WHERE RealName like '%" + uName + "%' ";
            }
            int total = 0;
            //string dept = (string)Session["Dept"];
            IEnumerable<UsersMore> temp = null; ;
            string sql = "SELECT *,(SELECT RoleName from Role WHERE users.RoleID=Role.ID )AS RoleName, (SELECT DeptName from Dept WHERE users.DeptId=Dept.ID)AS DeptName FROM users " + sqlUserName + "  and IsDeleted=0 order by " + sort + " " + direction;
            temp = userService.SqlQueryMore(sql);
            total = temp.Count();
            var users = temp.Skip<UsersMore>((pageIndex - 1) * pageSize).Take<UsersMore>(pageSize);
            var data = new
            {
                total = total,
                rows = users.ToList<UsersMore>()
            };


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShowDeptList()
        {
            IEnumerable<Dept> temp = null; ;
            string sql = "SELECT * FROM Dept WHERE  IsDeleted= 0 ";

            temp = deptService.SqlQuery(sql);

            List<Item> tList = new List<Item>();


            foreach (var t in temp)
            {
                Item atom = new Item();
                atom.id = t.ID;
                atom.text = t.DeptName;
                tList.Add(atom);
            }

            return Json(tList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShowRoleList()
        {
            IEnumerable<Role> temp = null;
            string sql = null;

            sql = "SELECT * FROM Role ";


            temp = roleService.SqlQuery(sql);

            return Json(temp.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeptList()
        {
            return View();
        }
        public ActionResult DeptEdit()
        {
            string type = Request["type"];
            Session["DeptEditType"] = type;

            Guid id = new Guid();
            if (Request["type"] != "CreateLevelOne")
            {
                Session["ID"] = Request["ID"];
            }
            if (Request["type"] == "Edit")
                id = new Guid(Request["ID"]);
            if (type != "Edit")
            {
                Dept dept = new Dept();
                ViewBag.dept = dept;
                return View();
            }
            else
            {
                Dept dept = deptService.Find(id);
                ViewBag.dept = dept;
                return View();
            }
        }
        public ActionResult DeptSaveOrUpdate(Dept item)
        {

            string editType = (string)Session["DeptEditType"];
            if (editType != "Edit")
            {
                Guid guid = Guid.NewGuid();
                item.ID = guid;
                item.IsDeleted = false;
                deptService.Add(item);
                return Content("true");
            }
            else
            {
                item.IsDeleted = false;
                deptService.Update(item);
                return Content("true");
            }

        }
        public ActionResult DeptDelete(Guid ID)
        {

            deptService.Delete(ID);
            return Content("true");
        }
        DeptGridTree deptGridTree = new DeptGridTree();
        public ActionResult GetDeptList()
        {

            return Content(deptGridTree.GetJson());
        }

        public ActionResult RoleList()
        {
            return View();
        }
        public ActionResult RoleEdit()
        {
            string type = Request["type"];
            Session["RoleEditType"] = type;
            Guid id = new Guid();
            if (Request["type"] != "Create")
                id = new Guid(Request["ID"]);
            if (type == "Create")
            {
                Role role = new Role();
                ViewBag.role = role;
                return View();
            }
            else
            {
                Role role = roleService.Find(id);
                ViewBag.role = role;
                return View();
            }
        }
        public ActionResult GetMenuComboTree(Guid ID)
        {
            return Content(MenuComboTree.GetJson(ID));
        }
        public ActionResult GetDeptComboTree(Guid ID)
        {
            return Content(DeptComboTree.GetJson(ID));
        }
        public ActionResult RoleSaveOrUpdate(Role item)
        {

            string editType = (string)Session["RoleEditType"];
            string menuTree = Request["menutree"];
            Guid id;
            if (editType != "Edit")
            {
                Guid guid = Guid.NewGuid();
                id = guid;
                item.ID = guid;

                roleService.Add(item);
            }
            else
            {
                id = item.ID;
                roleService.Update(item);
            }
            //string sql="Delete From RoleMenu Where roleId='"+id.ToString()+"'";
            //db.Database.ExecuteSqlCommand(sql);
            RoleMenuService.DeleteByRoleID(id);
            string[] array = menuTree.Split(',');
            foreach (string s in array)
            {
                RoleMenu RoleMenu = new RoleMenu();
                RoleMenu.ID = Guid.NewGuid();
                RoleMenu.RoleID = id;
                RoleMenu.MenuID = new Guid(s);
                RoleMenuService.Add(RoleMenu);

            }

            return Content("true");
        }
        public ActionResult RoleDelete(Guid ID)
        {
            roleService.Delete(ID);
            return Content("true");
        }
        public JsonResult GetRoleList()
        {
            int pageIndex = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            int pageSize = Request["rows"] == null ? 30 : int.Parse(Request["rows"]);
            string sort = Request["sort"];
            sort = sort == null ? sort = "ID" : sort;
            string direction = Request["order"];
            string uName = Request["uName"];
            string sqlUserName = "";
            if (uName != null)
            {
                sqlUserName = " and UserName like '%" + uName + "%'  and IsDeleted= 0 ";
            }
            else
            {
                sqlUserName = " Where IsDeleted= 0 ";
            }
            int total = 0;
            //string dept = (string)Session["Dept"];
            IEnumerable<Role> temp = null; ;
            string sql = "SELECT * FROM Role " + sqlUserName + " order by " + sort + " " + direction;
            temp = roleService.SqlQuery(sql);
            total = temp.Count();
            var users = temp.Skip<Role>((pageIndex - 1) * pageSize).Take<Role>(pageSize);
            var data = new
            {
                total = total,
                rows = users.ToList<Role>()
            };


            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
