using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Model;
namespace zjw
{
    public class MenuBarDB
    {
        DBEntities db = new DBEntities();
        

        public IList<Menu> returnParentMenu(Guid ID)
        {

            IList<Menu> t = new List<Menu>();
            IEnumerable<Menu> temp = null;
            string sql = "SELECT Menu.* FROM Users,Role,RoleMenu,Menu WHERE Users.RoleID=Role.ID and Role.ID=RoleMenu.roleId and RoleMenu.menuId=Menu.Id and ParentID<>'00000000-0000-0000-0000-000000000000' and Users.ID='" + ID + "' group by ParentID order by sequence";
            temp = db.Database.SqlQuery<Menu>(sql);
            var result = temp.ToList();
            List <Menu> root=new  List<Menu>();
            foreach (var find in result)
            {
                var i=from j in db.Menu where j.ID==find.ParentID select j;
                root.Add(i.FirstOrDefault());
            }
            return root;
        }

        public bool isHaveChild(Guid ID, Guid pid)
        {
            bool flag = false;
            IEnumerable<Menu> temp = null;
            string sql = "SELECT Menu.* FROM Users,Role,RoleMenu,Menu Where Users.RoleID=Role.ID and Role.ID=RoleMenu.roleId and RoleMenu.menuId=Menu.Id and Users.ID='" + ID + "' and parentId='" + pid + "' order by Menu.ID";
            temp = db.Database.SqlQuery<Menu>(sql);
            if (temp.FirstOrDefault()!=null)
            {
                flag = true;
            }
            return flag;

        }
        public IList<Menu> getChild(Guid ID, Guid pid)
        {
            IList<Menu> t = new List<Menu>();
            IEnumerable<Menu> temp = null;
            string sql = "SELECT Menu.* FROM Users,Role,RoleMenu,Menu Where Users.RoleID=Role.ID and Role.ID=RoleMenu.roleId and RoleMenu.menuId=Menu.Id and Users.ID='" + ID + "' and parentId='" + pid + "' order by Menu.ID";
            temp = db.Database.SqlQuery<Menu>(sql);
            return temp.ToList();
        }
    }
}