using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class StaticMesh : ClientAssetEntity, IListable, IDTOTransfer<StaticMeshDTO>
    {
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string SrcFileAssetId { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public List<Material> Materials { get; set; }

        public StaticMesh()
        {
            Materials = new List<Material>();
        }

        public StaticMeshDTO ToDTO()
        {
            var dto = new StaticMeshDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.PackageName = PackageName;
            dto.Description = Description;
            dto.FileAssetId = FileAssetId;
            dto.Dependencies = Dependencies;
            dto.Properties = Properties;
            dto.UnCookedAssetId = UnCookedAssetId;
            dto.SrcFileAssetId = SrcFileAssetId;
            dto.OrganizationId = OrganizationId;
            dto.Creator = Creator;
            dto.Modifier = Modifier;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.CreatorName = CreatorName;
            dto.ModifierName = ModifierName;
            dto.CategoryName = CategoryName;
            if (Materials != null && Materials.Count > 0)
                dto.Materials = Materials.Select(x => x.ToDTO()).ToList();
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

            return dto;
        }
    }


    public class StaticMeshDTO : ClientAssetEntity
    {
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string FileAssetId { get; set; }
        public string SrcFileAssetId { get; set; }
        public string Url { get; set; }
        public FileAssetDTO FileAsset { get; set; }
        public List<MaterialDTO> Materials { get; set; }
    }
}
