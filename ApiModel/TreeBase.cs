namespace ApiModel
{
    public class TreeBase : EntityBase, ITree
    {
        public int LValue { get; set; }
        public int RValue { get; set; }
        public string ParentId { get; set; }
        public string NodeType { get; set; }
        public string ObjId { get; set; }
        public string OrganizationId { get; set; }
    }
}
