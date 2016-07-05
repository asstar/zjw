using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using IBLL;
namespace BLL
{
    public class UserDeptService : BaseService<UserDept>, IUserDeptService
    {
        public void DeleteByUserID(Guid userId)
        {
            using (var db = new DBEntities())
            {
                var userDeptList = from r2m in db.UserDept where r2m.UserID == userId select r2m;
                List<UserDept> r2mList = userDeptList.ToList<UserDept>();
                for (int i = 0; i < r2mList.Count; i++)
                {
                    Delete(r2mList[i]);
                }

            }
        }
    }
}
