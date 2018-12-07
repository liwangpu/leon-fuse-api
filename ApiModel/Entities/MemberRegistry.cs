using System.ComponentModel.DataAnnotations.Schema;
namespace ApiModel.Entities
{
    public class MemberRegistry : EntityBase, IListable, IDTOTransfer<MemberRegistryDTO>
    {
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Company { get; set; }
        public string Icon { get; set; }
        public string BusinessCard { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public MemberRegistryDTO ToDTO()
        {
            var dto = new MemberRegistryDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.CategoryId = CategoryId;
            dto.OrganizationId = OrganizationId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.Icon = Icon;
            return dto;
        }
    }

    public class MemberRegistryDTO : EntityBase, IListable
    {
        public string Icon { get; set; }
        public FileAsset IconFileAsset { get; set; }
    }
}
