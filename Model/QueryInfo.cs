using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    [Serializable]
    public class QueryInfo
    {
        public int PageIndex{get;set;}
        public int PageSize{get;set;}
        public string Sort{get;set;}
        public string Direction{get;set;}
        public string QueryType { get; set; }
        public string KeyWord { get; set; }
        public string QueryString { get; set; }

        public QueryInfo deepClone()
        {
            QueryInfo clone = new QueryInfo();
            clone.PageIndex = this.PageIndex;
            clone.PageSize = this.PageSize;
            clone.Sort = this.Sort == null?null: new string(this.Sort.ToCharArray());
            clone.Direction = this.Direction == null ? null : new string(this.Direction.ToCharArray());
            clone.QueryType = this.QueryType == null ? null : new string(this.QueryType.ToCharArray());
            clone.KeyWord = this.KeyWord == null ? null : new string(this.KeyWord.ToCharArray());
            clone.QueryString = this.QueryString == null ? null : new string(this.QueryString.ToCharArray());
            return clone;
        }
    }
}