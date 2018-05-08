using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Material : EntityBase, IAsset, IPermission, IListable, IDTOTransfer<MaterialDTO>
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }

        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }

        [NotMapped]
        public FileAsset FileAsset { get; set; }


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
            dto.CategoryId = CategoryId;

            return dto;
        }
    }

    public class MaterialDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Url { get; set; }
        public FileAssetDTO FileAsset { get; set; }
        public string CategoryId { get; set; }
    }
}
