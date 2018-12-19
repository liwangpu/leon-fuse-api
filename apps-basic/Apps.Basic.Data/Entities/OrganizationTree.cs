using Apps.Base.Common.Interfaces;

namespace Apps.Basic.Data.Entities
{
    public class OrganizationTree : ITree
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
