using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApiModel.Entities
{
    public class Map : EntityBase, IListable
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        [NotMapped]
        public FileAsset FileAsset { get; set; }

        public string Dependencies { get; set; }
        public string Properties { get; set; }

        public override Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = Id;
            dicData["Name"] = Name;
            dicData["CreatedTime"] = CreatedTime;
            dicData["ModifiedTime"] = ModifiedTime;
            if (FileAsset != null)
            {
                dicData["Url"] = FileAsset.Url;
            }
            dicData["Dependencies"] = Dependencies;
            dicData["Properties"] = Properties;

            return dicData;
        }
    }
}
