using Apps.Base.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.MoreJee.Data.Entities
{
    public class AssetCategoryTree : ITree
    {
        public string Id { get; set; }
        public int LValue { get; set; }
        public int RValue { get; set; }
        public string ParentId { get; set; }
        public string NodeType { get; set; }
        public string ObjId { get; set; }
        public string OrganizationId { get; set; }
    }
}
