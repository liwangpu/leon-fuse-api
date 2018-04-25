using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Material : EntityBase, IListable, IDTOTransfer<MaterialDTO>
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }


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
            dicData["Parameters"] = Parameters;

            return dicData;
        }

        public MaterialDTO ToDTO()
        {
            var dto = new MaterialDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
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

            return dto;
        }
    }

    public class MaterialDTO : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Url { get; set; }
        public FileAssetDTO FileAsset { get; set; }
    }
}
