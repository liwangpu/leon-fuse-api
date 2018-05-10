using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class StaticMesh : EntityBase, IListable, IDTOTransfer<IData>
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public List<Material> Materials { get; set; }


        public StaticMesh()
        {
            Materials = new List<Material>();
        }

        public IData ToDTO()
        {
            var dto = new StaticMeshDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
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
            return dto;
        }
    }


    public class StaticMeshDTO : DataBase
    {
        public string Icon { get; set; }
        public string Description { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string FileAssetId { get; set; }
        public string Url { get; set; }
        public IData FileAsset { get; set; }
        public List<IData> Materials { get; set; }
    }
}
