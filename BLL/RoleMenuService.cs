using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class RoleMenuService : BaseService<RoleMenu>, IRoleMenuService
    {
        public void DeleteByRoleID(Guid roleId)
        {
            using (var db = new DBEntities())
            {
                var roleToMenuList = from r2m in db.RoleMenu where r2m.RoleID == roleId select r2m;
                List<RoleMenu> r2mList = roleToMenuList.ToList<RoleMenu>();
                for (int i = 0; i < r2mList.Count; i++)
                {
                    Delete(r2mList[i]);
                }

            }
        }
    }
}
