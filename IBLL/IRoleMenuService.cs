using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
namespace IBLL
{
    public interface IRoleMenuService : IBaseService<RoleMenu>
    {
        void DeleteByRoleID(Guid roleId);
    }
}
