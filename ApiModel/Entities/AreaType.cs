using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class AreaType : EntityBase, IListable, IDTOTransfer<AreaTypeDTO>
    {
        public string Icon { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public AreaTypeDTO ToDTO()
        {
            var dto = new AreaTypeDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.OrganizationId = OrganizationId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            return dto;
        }
    }

    public class AreaTypeDTO : EntityBase
    {
        public string IconAssetId { get; set; }
        public string Icon { get; set; }
    }
}
