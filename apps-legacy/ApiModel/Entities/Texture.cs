using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Texture : ClientAssetEntity, IListable, IDTOTransfer<TextureDTO>
    {
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string FileAssetId { get; set; }

        [NotMapped]
        public string FileAssetUrl { get; set; }
  
        public TextureDTO ToDTO()
        {
            var dto = new TextureDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.PackageName = PackageName;
            dto.Description = Description;
            dto.UnCookedAssetId = UnCookedAssetId;
            dto.OrganizationId = OrganizationId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.Dependencies = Dependencies;
            dto.Properties = Properties;
            dto.CategoryName = CategoryName;

            dto.Icon = IconFileAssetUrl;
            dto.IconAssetId = Icon;
            dto.FileAssetId = FileAssetId;
            dto.FileAssetUrl = FileAssetUrl;

            return dto;
        }
    }

    public class TextureDTO : ClientAssetEntity
    {
        public string IconAssetId { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string FileAssetUrl { get; set; }
    }
}
