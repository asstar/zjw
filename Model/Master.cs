//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Master
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string MasterType { get; set; }
        public string CaseName { get; set; }
        public string CaseCode { get; set; }
        public string UnderTakenDept { get; set; }
        public string UnderTaker { get; set; }
        public string TargetName { get; set; }
        public Nullable<System.DateTime> FormedDate { get; set; }
        public string Note { get; set; }
        public string Movie { get; set; }
        public Nullable<System.Guid> Affix { get; set; }
        public string TurnInAddr { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
    }
}
