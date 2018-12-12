using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class OrganizationType : EntityBase, IListable, IDTOTransfer<OrganizationTypeDTO>
    {
        public string TypeCode { get; set; }
        public string Icon { get; set; }
        public bool IsInner { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public OrganizationTypeDTO ToDTO()
        {
            var dto = new OrganizationTypeDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.ActiveFlag = ActiveFlag;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.TypeCode = TypeCode;
            dto.IsInner = IsInner;
            return dto;
        }
    }

    public class OrganizationTypeDTO : EntityBase, IListable
    {
        public string TypeCode { get; set; }
        public string Icon { get; set; }
        public string ApplyOrgans { get; set; }
        public bool IsInner { get; set; }
        public FileAsset IconFileAsset { get; set; }
    }

}
