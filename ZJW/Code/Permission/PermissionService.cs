using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Model;
using IBLL;
using BLL;
using System.Diagnostics;
namespace zjw
{
    public class PermissionService
    {
        PermissionTree tree = new PermissionTree();
        IUserDeptService userDeptService = new UserDeptService();
        IDeptService deptService = new DeptService();
        IUserService userService = new UserService();
        public string getPermission()
        {
            BaseInfo baseInfo = (BaseInfo)HttpContext.Current.Session["User"];
            List<Dept> deptList = new List<Dept>();
            if (baseInfo.user.IsKeyNode == false)
            {
                string sql = "Select * From UserDept where UserID='" + baseInfo.user.ID + "'";
                var list = userDeptService.SqlQuery(sql);
                foreach (var item in list)
                {
                    Guid userID = (Guid)(deptService.Find((Guid)item.DeptID)).UserID;
                    deptList.AddRange(tree.getCurrentNodeAndChildrenNodes(userID));
                }

            }
            else
            {
                Guid userID = baseInfo.user.ID;
                deptList = tree.getCurrentNodeAndChildrenNodes(userID);
            }
            List<Users> userGroup = new List<Users>();
            if (baseInfo.role.RoleName == "专案组")
            {
                userGroup.Add(baseInfo.user);
            }
            else
            {
                foreach (var dept in deptList)
                {
                    List<Users> usersList = userService.SqlQuery("Select * From Users Where DeptID='" + dept.ID + "'").ToList();
                    foreach (var item in usersList)
                    {
                        userGroup.Add(item);
                    }
                }
            }
            string result = null; ;
            if (userGroup.Count != 0)
            {

                result = " AND (";
                foreach (var r in userGroup)
                {
                    result += "UserID='" + r.ID + "' /*" + r.RealName + "*/ OR ";
                    //Debug.WriteLine("ID:" + r.ID + "RealName:" + r.RealName);
                }
                result = result.Substring(0, result.LastIndexOf("OR"));
                result += ")";
                Debug.WriteLine(result);
            }
            return result;
        }

        //public 
    }
}