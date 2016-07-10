using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class BtnModel
    {
        public BtnModel()
        {
            isTransferable = false;
            isOutable = false;
            isReturnable = false;
            isHandleable = false;
            isDeliverable = false;
            isBorrowable = false;
        }
        public string type { get; set; }
        public bool isTransferable { get; set; }
        public bool isOutable { get; set; }
        public bool isReturnable { get; set; }
        public bool isHandleable { get; set; }
        public bool isEditable { get; set; }
        public bool isDeliverable { get; set; }
        public bool isBorrowable { get; set; }


        public string getBtnString()
        {
            string result = null;
            if (isEditable)
            {
                if (result != null)
                {
                    result += ", \"-\", ";
                }
                result += "{ text: \"批量编辑\", iconCls: \"icon-add\", handler: function () { batchEdit(\"批量编辑\"); }  }";
            }
            if (isTransferable)
            {
                if (result != null)
                {
                    result += ", \"-\", ";
                }
                result += "{ text: \"移交\", iconCls: \"icon-add\", handler: function () { submitSelected(\"移交\"); }  }";
            }
            if (isOutable)
            {
                if (result != null)
                {
                    result += ", \"-\", ";
                }
                result += "{ text: \"出库\", iconCls: \"icon-add\", handler: function () { submitSelected(\"出库\"); }  }";
            }
            if (isReturnable)
            {
                if (result != null)
                {
                    result += ", \"-\", ";
                }
                result += "{ text: \"返库\", iconCls: \"icon-add\", handler: function () { submitSelected(\"返库\"); }  }";
            }
            if (isHandleable)
            {
                if (result != null)
                {
                    result += ", \"-\", ";
                }
                result += "{ text: \"处置\", iconCls: \"icon-add\", handler: function () { submitSelected(\"处置\"); }  }";
            }
            if (isDeliverable)
            {
                if (result != null)
                {
                    result += ", \"-\", ";
                }
                result += "{ text: \"提交审理\", iconCls: \"icon-add\", handler: function () { submitSelected(\"提交审理\"); }  }";

            }
            if (isBorrowable)
            {
                if (result != null)
                {
                    result += ", \"-\", ";
                }
                result += "{ text: \"调取\", iconCls: \"icon-add\", handler: function () { submitSelected(\"调取\"); }  }";
            }
            return result;
        }
    }
}
