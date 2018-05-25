using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Texture : ClientAssetEntity, IListable, IDTOTransfer<TextureDTO>
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }

        public TextureDTO ToDTO()
        {
            var dto = new TextureDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.Modifier = Modifier;
            dto.Dependencies = Dependencies;
            dto.Properties = Properties;
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            if (FileAsset != null)
                dto.FileAsset = FileAsset.ToDTO();

            return dto;
        }
    }

    public class TextureDTO : EntityBase
    {
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public FileAssetDTO FileAsset { get; set; }
    }
}
