using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class Product : EntityBase, IAsset, IDTOTransfer<ProductDTO>
    {
        public string Icon { get; set; }
        public string CategoryId { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [JsonIgnore]
        public List<ProductSpec> Specifications { get; set; }

        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        [NotMapped]
        public AssetCategory AssetCategory { get; set; }

        public ProductDTO ToDTO()
        {
            var dto = new ProductDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.CategoryId = CategoryId;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            if (Specifications != null && Specifications.Count > 0)
                dto.Specifications = Specifications.Select(x => x.ToDTO()).ToList();
            if (IconFileAsset != null)
            {
                dto.Icon = IconFileAsset.Url;
                dto.IconAssetId = IconFileAsset.Id;
            }

            if (AssetCategory != null)
                dto.CategoryName = AssetCategory.Name;
            return dto;
        }
    }

    public class ProductDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string IconAssetId { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public List<ProductSpecDTO> Specifications { get; set; }
    }

}
