using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
using System.Data;
namespace BLL
{
    public class CaseService: BaseService<Case>, ICaseService
    {
        public List<Case> List(ListModel listModel, String TableName, ref int total)
        {
            using (var db = new DBEntities())
            {
                string sql = "select * FROM " + TableName + " where isDeleted=false " + listModel.StatusString + listModel.QueryString + listModel.AuthString + " order by " + listModel.Sort + " " + listModel.Direction;
                IEnumerable<Case> temp = SqlQuery(sql);
                total = temp.Count();
                var users = temp.Skip<Case>((listModel.PageIndex - 1) * listModel.PageSize).Take<Case>(listModel.PageSize);
                var result = users.ToList<Case>();
                return result;
            }
        }

    }
}
