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
    [Serializable]
    public class BaseInfo
    {
        public Users user { get; set; }
        public Role role { get; set; }
        public Dept dept { get; set; }
    }
}