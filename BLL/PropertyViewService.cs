using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class PropertyViewService : BaseService<PropertyView>, IPropertyViewService
    {
        public PropertyView FindLinkID(Guid id)
        {
            var sql = "Select * From PropertyView Where LinkID='" + id.ToString() + "'";
            return SqlQuery(sql).FirstOrDefault();
        }
    }
}
