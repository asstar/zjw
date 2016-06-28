using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Model;
namespace zjw
{
    public class DeptGridTree
    {
        DeptGridTreeDB deptGridTreeDB = new DeptGridTreeDB();
        public string GetJson()
        {
            string json = "[";
            IList<Dept> t = deptGridTreeDB.returnParentDept();
            foreach (Dept model in t)
            {
                if (model != t[t.Count - 1])
                {
                    json += GetJsonByModel(model) + ",";
                }
                else
                {
                    json += GetJsonByModel(model);
                }
            }
            json += "]";
            Debug.WriteLine(json);
            //json = json.Replace("#", "'");
            json = json.Replace("k__BackingField", "");
            json = json.Replace("<", "");
            json = json.Replace(">", "");
            return json;
        }

        public string GetJsonByModel(Dept t)
        {
            string json = "";
            bool flag = deptGridTreeDB.isHaveChild(t.ID);

            json = JSONHelper.GetJSON<Dept>(t);
            
            if(json.Substring(json.Length-1,1)=="}")
              json = json.Substring(0, json.Length - 1);
            json += ",\"children\":";
            if (!flag)
            {
                json += "null}";
            }
            else
            {
                json += "[";
                IList<Dept> list = deptGridTreeDB.getChild(t.ID);
                foreach (Dept atom in list)
                {
                    if (atom != list[list.Count - 1])
                    {
                        json += GetJsonByModel(atom) + ",";
                    }
                    else
                    {
                        json += GetJsonByModel(atom);
                    }
                }
                json += "]}";
            }
            json = json.Replace("k__BackingField", "");
            json = json.Replace("<", "");
            json = json.Replace(">", "");
            return json;
        }
    
    }
}