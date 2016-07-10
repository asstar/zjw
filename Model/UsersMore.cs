using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class UsersMore
    {
        public System.Guid ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<System.Guid> DeptID { get; set; }
        public string RealName { get; set; }
        public Nullable<System.Guid> RoleID { get; set; }
        public string Rank { get; set; }
        public bool IsDeleted { get; set; }
        public string RoleName { get; set; }
        public string DeptName { get; set; }
        public string RankName { get; set; }
    }
}
