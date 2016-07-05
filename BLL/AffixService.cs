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
    public class AffixService : BaseService<Affix>, IAffixService
    {
        public void AddMore(Affix affix)
        {
            String sql = "select * from affix where MasterID='" + affix.MasterID + "' and next is null and Type='"+affix.Type+"'";
            var result = SqlQuery(sql).FirstOrDefault();
            if (result != null)
            {
                affix.Prev = result.ID;
                result.Next = affix.ID;
                Add(affix);
                Update(result);
            }
            else
            {
                Add(affix);
            }
        }

    }
}
