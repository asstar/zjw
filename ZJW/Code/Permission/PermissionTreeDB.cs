using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Model;
namespace zjw
{
    public class PermissionTreeDB
    {
        public bool isHaveChild(Guid id)
        {
            using (var db = new DBEntities())
            {
                bool flag = false;
                IEnumerable<Dept> temp = null;
                string sql = "select * from Dept where ParentId='" + id + "'";
                temp = db.Database.SqlQuery<Dept>(sql);
                if (temp.FirstOrDefault() != null)
                {
                    flag = true;
                }
                return flag;
            }

        }
        public IList<Dept> getChild(Guid pid)
        {
            using (var db = new DBEntities())
            {
                IList<Dept> t = new List<Dept>();
                IEnumerable<Dept> temp = null;
                string sql = "SELECT * FROM Dept WHERE  ParentId='" + pid + "' ORDER BY Sequence";
                temp = db.Database.SqlQuery<Dept>(sql);
                return temp.ToList();
            }
        }
    }

}