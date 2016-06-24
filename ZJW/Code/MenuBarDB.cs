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

        public IList<Menu> returnParentMenu()
        {

            IList<Menu> t = new List<Menu>();
            IEnumerable<Menu> temp = null;
            string sql = "SELECT * FROM Menu WHERE  ParentID='00000000-0000-0000-0000-000000000000' order by sequence";
            temp = db.Database.SqlQuery<Menu>(sql);
            var result = temp.ToList();
            return result;
        }

        public bool isHaveChild(Guid ID, Guid pid)
        {
            bool flag = false;
            IEnumerable<Menu> temp = null;
            string sql = "SELECT Menu.* FROM Users,Role,RoleMenu,Menu Where Users.Role=Role.ID and Role.ID=RoleMenu.roleId and RoleMenu.menuId=Menu.Id and Users.ID='" + ID + "' and parentId='" + pid + "' order by Menu.ID";
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
            string sql = "SELECT Menu.* FROM Users,Role,RoleMenu,Menu Where Users.Role=Role.ID and Role.ID=RoleMenu.roleId and RoleMenu.menuId=Menu.Id and Users.ID='" + ID + "' and parentId='" + pid + "' order by Menu.ID";
            temp = db.Database.SqlQuery<Menu>(sql);
            return temp.ToList();
        }
    }
}