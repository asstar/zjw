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
    
    public partial class Money
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CaseID { get; set; }
        public string CaseName { get; set; }
        public string CaseCode { get; set; }
        public string MoneyCode { get; set; }
        public string OriginalHolder { get; set; }
        public string MoneyProperty { get; set; }
        public string MoneyType { get; set; }
        public string Sum { get; set; }
        public string OriginAccount { get; set; }
        public string KeepAccount { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> WithholdDate { get; set; }
        public string UnderTakenStaff { get; set; }
        public Nullable<System.DateTime> HandOverDate { get; set; }
        public string ReceiveStaff { get; set; }
        public string HandleMethod { get; set; }
        public Nullable<System.DateTime> HandleDate { get; set; }
        public string HandleReveiveStaff { get; set; }
        public string Note { get; set; }
        public string Affix { get; set; }
    }
}
