using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IUserService : IBaseService<Users>
    {
        Users FindByUserName(string userName);
        IEnumerable<UsersMore> SqlQueryMore(string sql);
        Users FindByMasterID(Guid masterID);
    }
}
