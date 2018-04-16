using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class StaticMesh : EntityBase, IListable
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ProductSpecId { get; set; }
        public ProductSpec ProductSpec { get; set; }
        public string FileAssetId { get; set; }
        [NotMapped]
        public FileAsset FileAsset { get; set; }
        public List<Material> Materials { get; set; }
        public override Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = Id;
            dicData["Name"] = Name;
            dicData["CreatedTime"] = CreatedTime;
            dicData["ModifiedTime"] = ModifiedTime;
            dicData["Description"] = Description;
            dicData["Icon"] = Icon;
            if (FileAsset != null)
            {
                dicData["url"] = FileAsset.Url;
            }
            if (Materials != null && Materials.Count > 0)
            {
                dicData["Materials"] = Materials.Select(x => x.ToDictionary());
            }
            return dicData;
        }
    }
}
