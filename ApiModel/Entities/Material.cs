using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Material : ClientAssetEntity, IAsset, IListable, IDTOTransfer<MaterialDTO>
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }
        public string CategoryId { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        [NotMapped]
        public AssetCategory AssetCategory { get; set; }

        public MaterialDTO ToDTO()
        {
            var dto = new MaterialDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.FileAssetId = FileAssetId;
            dto.Dependencies = Dependencies;
            dto.Parameters = Parameters;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            if (FileAsset != null)
            {
                dto.FileAsset = FileAsset.ToDTO();
                dto.Url = FileAsset.Url;
            }

            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }
            if (AssetCategory != null)
                dto.CategoryName = AssetCategory.Name;
            dto.CategoryId = CategoryId;

            return dto;
        }
    }

    public class MaterialDTO :EntityBase
    {
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }
        public string Url { get; set; }
        public FileAssetDTO FileAsset { get; set; }
        public string CategoryId { get; set; }
    }
}
