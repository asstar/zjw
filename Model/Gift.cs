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
    
    public partial class Gift
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string TurnInCode { get; set; }
        public string UnderTakenDept { get; set; }
        public string UnderTaker { get; set; }
        public string TargetName { get; set; }
        public Nullable<System.DateTime> TurnInDate { get; set; }
        public string TurnInAddr { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }
        public Nullable<System.Guid> Affix { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
    }
}
