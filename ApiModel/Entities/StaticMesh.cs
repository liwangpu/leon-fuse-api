using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class StaticMesh : EntityBase, IListable
    {
        public string Description { get; set; }
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
                dicData["Url"] = FileAsset.Url;
            }
            if (Materials != null && Materials.Count > 0)
            {
                dicData["Materials"] = Materials.Select(x => x.ToDictionary());
            }
            dicData["Dependencies"] = Dependencies;
            dicData["Properties"] = Properties;

            return dicData;
        }
    }
}
