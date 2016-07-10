using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace zjw
{
    public static class Utils
    {
        public static string ConvertDateToString(DateTime? src)
        {
            DateTime tmp = new DateTime();
            string dest = null;
            if (src != null)
            {
                tmp = (DateTime)src;
                dest = tmp.Year + "年" + tmp.Month + "月" + tmp.Day + "日";
            }          
            return dest;
        }
    }
}