namespace ApiModel
{
    public interface ITree : IData
    {
        int LValue { get; set; }
        int RValue { get; set; }
        string ParentId { get; set; }
        string NodeType { get; set; }
        string ObjId { get; set; }
        string OrganizationId { get; set; }
    }
}
