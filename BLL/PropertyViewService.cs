using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
using System.Diagnostics;
namespace BLL
{
    public class PropertyViewService : BaseService<PropertyView>, IPropertyViewService
    {
        public PropertyView FindLinkID(Guid id)
        {
            var sql = "Select * From PropertyView Where LinkID='" + id.ToString() + "'";
            return SqlQuery(sql).FirstOrDefault();
        }
        public List<PropertyView> List(ListModel listModel, String TableName, ref int total)
        {
            using (var db = new DBEntities())
            {
                string sql = "select * FROM " + TableName + " where isDeleted=false " + listModel.StatusString + listModel.QueryString + listModel.AuthString + " order by " + listModel.Sort + " " + listModel.Direction;
                Debug.WriteLine(sql);
                IEnumerable<PropertyView> temp = SqlQuery(sql);
                var result = temp.ToList<PropertyView>();
                Dictionary<Guid, PropertyView> dict = new Dictionary<Guid, PropertyView>();
                List<PropertyView> process = new List<PropertyView>();
                foreach (var i in result)
                {
                    PropertyView trace = i;
                    while (trace.Next != null)
                    {
                        trace = FindLinkID((Guid)trace.Next);
                    }
                    if (!dict.ContainsKey(trace.LinkID))
                    {
                        dict.Add(trace.LinkID, trace);
                        process.Add(trace);
                    }
                }
                total = process.Count();
                var users = process.Skip<PropertyView>((listModel.PageIndex - 1) * listModel.PageSize).Take<PropertyView>(listModel.PageSize);

                return users.ToList();
            }
        }
    }
}
