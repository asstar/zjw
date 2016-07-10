using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class GoodsViewService : BaseService<GoodsView>, IGoodsViewService
    {
        public GoodsView FindLinkID(Guid id)
        {
            var sql = "Select * From GoodsView Where LinkID='" + id.ToString() + "'";
            return SqlQuery(sql).FirstOrDefault();
        }
    }
}
