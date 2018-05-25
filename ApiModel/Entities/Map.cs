using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Map : ClientAssetEntity, IListable, IDTOTransfer<MapDTO>
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }


        public MapDTO ToDTO()
        {
            var dto = new MapDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.PackageName = PackageName;
            dto.Dependencies = Dependencies;
            dto.Properties = Properties;
            dto.Description = Description;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            if (FileAsset != null)
                dto.FileAsset = FileAsset.ToDTO();
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }

            return dto;
        }
    }

    public class MapDTO : EntityBase
    {
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string PackageName { get; set; }
        public FileAssetDTO FileAsset { get; set; }
    }


}
