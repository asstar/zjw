using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using IBLL;
using BLL;
using System.Diagnostics;
namespace zjw
{
    public class PermissionTree
    {
        PermissionTreeDB currentLevelGridTreeDB = new PermissionTreeDB();
        IUserService userService = new UserService();
        IDeptService deptService = new DeptService();
        IRoleService roleService = new RoleService();
        List<Dept> deptList = new List<Dept>();
        //List<Guid> result = new List<Guid>();
        public List<Dept> getCurrentNodeAndChildrenNodes(Guid userID)
        {
            //result.Add(userID);
            deptList.Clear();
            Users user = userService.Find(userID);
            Dept dept = deptService.Find((Guid)user.DeptID);
            Role role=roleService.Find((Guid)user.RoleID);

            //deptList.Add(dept);
            TraverseFromNode(dept);

            return deptList;
        }
        public void TraverseFromNode(Dept t)
        {

            bool flag = currentLevelGridTreeDB.isHaveChild(t.ID);
            if (!flag)
            {
                deptList.Add(t);
            }
            else
            {
                deptList.Add(t);
                //IList<Dept> list = globalGridTreeDB.getChild(t.ID);
                IList<Dept> list = currentLevelGridTreeDB.getChild(t.ID);
                foreach (Dept atom in list)
                {
                    TraverseFromNode(atom);
                }
            }
        }
    }
}