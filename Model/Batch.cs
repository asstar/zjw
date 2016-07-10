using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Batch
    {
        public string Title { get;set; }
        public string Action { get;set; }

        public string BtnTitle { get; set; }
        public string ReasonTitle { get; set; }

    }
}
