namespace ApiModel.Entities
{
    public class PermissionTree : IDTOTransfer<PermissionTree>,IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int LValue { get; set; }
        public int RValue { get; set; }
        public string ParentId { get; set; }
        public string NodeType { get; set; }
        public string ObjId { get; set; }
        public string OrganizationId { get; set; }

        public PermissionTree ToDTO()
        {
            return new PermissionTree();
        }
    }
}
