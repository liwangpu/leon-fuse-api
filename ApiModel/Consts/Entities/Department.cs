using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class Department : EntityBase, IListable, IDTOTransfer<DepartmentDTO>
    {
        public string ParentId { get; set; }
        public Department Parent { get; set; }
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public List<OrganMember> Members { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public DepartmentDTO ToDTO()
        {
            var dto = new DepartmentDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
            dto.ParentId = ParentId;
            return dto;
        }
    }


    public class DepartmentDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ParentId { get; set; }
    }
}
