using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using Model;
namespace zjw
{
    public class MenuBar
    {
        public static Guid GID;
        public static string GetJson(Guid ID)
        {
            GID = ID;
            string json = "{\"menus\":[";
            IList<Menu> t = new MenuBarDB().returnParentMenu(GID);
            foreach (Menu model in t)
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
            json += "]}";

            json = json.Replace(@"\/", "/");
            //json = json.Replace("\"", "'");
            Debug.WriteLine(json);
            return json;
        }

        public static string GetJsonByModel(Menu t)
        {
            string json = "";
            bool flag = new MenuBarDB().isHaveChild(GID, (Guid)(t.ID));
            if (t.Url == "/")
                json = "{\"menuid\": \"" + t.ID + "\",\"icon\":\"" + t.Icon + "\",\"menuname\":\"" + t.Name + "\"";
            else
                json = "{\"menuid\": \"" + t.ID + "\",\"icon\":\"" + t.Icon + "\",\"menuname\":\"" + t.Name + "\",\"url\":\"" + t.Url + "\"";

            if (!flag)
            {
                json += "}";
            }
            else
            {
                json += ",\"menus\":";
                json += "[";
                IList<Menu> list = new MenuBarDB().getChild(GID, t.ID);
                foreach (Menu atom in list)
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