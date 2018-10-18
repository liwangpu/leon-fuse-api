namespace ApiModel.Entities
{
    public class Navigation : TreeBase, IDTOTransfer<NavigationDTO>
    {
        public string Role { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Permission { get; set; }
        public string PagedModel { get; set; }
        public string Resource { get; set; }

        public NavigationDTO ToDTO()
        {
            var dto = new NavigationDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Resource = Resource;
            dto.NodeType = NodeType;
            dto.ParentId = ParentId;
            dto.Role = Role;
            dto.Url = Url;
            dto.Icon = Icon;
            dto.LValue = LValue;
            dto.RValue = RValue;
            dto.Permission = Permission;
            dto.PagedModel = PagedModel;
            return dto;
        }
    }

    public class NavigationDTO : TreeBase
    {
        public string Role { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Permission { get; set; }
        public string PagedModel { get; set; }
        public string Resource { get; set; }
    }
}
