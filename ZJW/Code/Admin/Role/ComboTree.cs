using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Model;
namespace zjw
{
    public class ComboTree
    {
        public static Guid GID;
        public static string GetJson(Guid ID)
        {
            GID = ID;
            string json = "[";
            IList<Tree> t = ComboTreeDB.returnParentTreeModel(GID);
            foreach (Tree model in t)
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
            return json;
        }

        public static string GetJsonByModel(Tree t)
        {
            string json = "";
            bool flag = ComboTreeDB.isHaveChild(t.id);

            json = "{"
                      + "\"id\":\"" + t.id + "\","
                      + "\"text\":\"" + t.text + "\","
                      + "\"iconCls\":\"ok\",";
            if (t.@checked != null)
            {
                json += "\"checked\":true,";
            }
            json += "\"children\":";
            if (!flag)
            {
                json += "null}";
            }
            else
            {
                json += "[";
                IList<Tree> list = ComboTreeDB.getChild(GID,t.id);
                foreach (Tree atom in list)
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
            return json;
        }
    
    }
}