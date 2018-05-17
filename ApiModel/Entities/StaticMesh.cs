using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class StaticMesh : EntityBase, IListable, IDTOTransfer<StaticMeshDTO>
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public List<Material> Materials { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }

        public StaticMesh()
        {
            Materials = new List<Material>();
        }

        public StaticMeshDTO ToDTO()
        {
            var dto = new StaticMeshDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.FileAssetId = FileAssetId;
            dto.Dependencies = Dependencies;
            dto.Properties = Properties;
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


    public class StaticMeshDTO : DataBase
    {
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string Description { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string FileAssetId { get; set; }
        public string Url { get; set; }
        public FileAssetDTO FileAsset { get; set; }
        public List<MaterialDTO> Materials { get; set; }
    }
}
