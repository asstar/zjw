using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ExportForm
    {

        public System.Guid ID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string FormType { get; set; }
        public string Template { get; set; }
        public string FormedDate { get; set; }
        public string OriginHolder { get; set; }
        public string UnderTaker { get; set; }
        public string Keeper { get; set; }
        public string Amount { get; set; }
        public string UndertakenDept { get; set; }
        public string Reason { get; set; }
        public string UndertakenDeptOpinion { get; set; }
        public string UndertakenDeptOpinionDate { get; set; }
        public string LeaderOpinion { get; set; }
        public string LeaderOpinionDate { get; set; }
        public string KeeperOpinion { get; set; }
        public string KeeperOpinionDate { get; set; }
        public string Note { get; set; }
        public string SendDept { get; set; }
        public string SendLeader { get; set; }
        public string Sender { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string Receiver { get; set; }
        public string ReceiveDept { get; set; }
        public string ReceiveLeader { get; set; }
        public Nullable<System.DateTime> ReceiveDate { get; set; }
        public string Data { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
    }
}

