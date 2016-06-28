using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Model;
namespace zjw
{
    public class ComboTreeDB
    {
        static DBEntities db = new DBEntities();

        public static IList<Tree> returnParentTreeModel(Guid ID)
        {

            IList<Tree> t = new List<Tree>();
            IEnumerable<Tree> temp = null;
            string sql = "SELECT * FROM (SELECT menu.ID as id ,Name as text,parentId,r.checked FROM menu left join (select id AS checked From RoleMenu WHERE roleId='" + ID + "') AS r ON menu.Id=r.checked) AS s WHERE  parentId='00000000-0000-0000-0000-000000000000'";
            temp = db.Database.SqlQuery<Tree>(sql);
            return temp.ToList();
        }

        public static bool isHaveChild(Guid id)
        {
            bool flag = false;
            IEnumerable<Tree> temp = null;
            string sql = "select id from menu where parentId='" + id + "'";
            temp = db.Database.SqlQuery<Tree>(sql);
            if (temp.FirstOrDefault()!=null)
            {
                flag = true;
            }
            return flag;

        }
        public static IList<Tree> getChild(Guid ID, Guid pid)
        {
            IList<Tree> t = new List<Tree>();
            IEnumerable<Tree> temp = null;
            string sql ="SELECT * FROM (SELECT menu.id as id ,Name as text,parentId,r.checked FROM menu left join (select menuid AS checked From RoleMenu WHERE roleId='" + ID + "') AS r ON menu.Id=r.checked) AS s WHERE  parentId='"+pid+"'";
            temp = db.Database.SqlQuery<Tree>(sql);
            return temp.ToList();
        }
    }
}