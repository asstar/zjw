using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Transfer
    {
        public Guid UserID { get; set; }
        public string PrevStatus{get;set;}
        public string CurrentStatus{get;set;}

    }
}
