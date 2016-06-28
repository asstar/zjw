using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model;
using IBLL;
using BLL;
using System.Diagnostics;
namespace zjw
{
    
    public class UserInfo
    {
        public BaseInfo baseInfo { get; set; }
        public Users user { get; set; }
        public Role role { get; set; }
        public Dept dept { get; set; }
        [NonSerialized]
        DBEntities db = new DBEntities();
        IRoleService roleService = new RoleService();

        private void SerialData()
        {
            baseInfo.user = user;
            baseInfo.role = role;
            baseInfo.dept = dept;
        }
        public UserInfo(BaseInfo baseInfo)
        {
            this.baseInfo = baseInfo;
            user = baseInfo.user;
            role = baseInfo.role;
            dept = baseInfo.dept;
        }
        public UserInfo()
        {
            baseInfo = new BaseInfo();
        }
        public void setUser(Guid ID)
        {

            user = db.Users.Find(ID);
            var roleTemp = from t in db.Role where t.ID == user.RoleID select t;
            role = roleTemp.FirstOrDefault();
            var deptTemp = from d in db.Dept where d.ID == user.DeptID select d;
            dept = deptTemp.FirstOrDefault();
            SerialData();
        }
    }
}