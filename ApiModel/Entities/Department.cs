using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Department : EntityBase, IListable, IDTOTransfer<DepartmentDTO>
    {
        public string ParentId { get; set; }
        public Department Parent { get; set; }
        public Organization Organization { get; set; }
        public List<OrganMember> Members { get; set; }
        public string Icon { get; set; }

        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public DepartmentDTO ToDTO()
        {
            var dto = new DepartmentDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
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
