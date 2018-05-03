using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class ProductSpec : EntityBase, IListable, IDTOTransfer<ProductSpecDTO>
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string CharletIds { get; set; }
        public string StaticMeshIds { get; set; }

        /// <summary>
        /// 价格，单位为元
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 第三方ID，此产品在供应商自己的系统比如ERP的ID
        /// </summary>
        public string TPID { get; set; }

        /// <summary>
        /// 所在产品的ID
        /// </summary>
        public string ProductId { get; set; }
        public Product Product { get; set; }

        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        [NotMapped]
        public List<FileAsset> CharletAsset { get; set; }
        [NotMapped]
        public List<StaticMesh> StaticMeshAsset { get; set; }

        public ProductSpec()
        {
            CharletAsset = new List<FileAsset>();
            StaticMeshAsset = new List<StaticMesh>();
        }

        public ProductSpecDTO ToDTO()
        {
            var dto = new ProductSpecDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Description = Description;
            dto.Icon = Icon;
            dto.CreatedTime = CreatedTime;
            dto.ModifiedTime = ModifiedTime;
            dto.Price = Price;
            dto.TPID = TPID;
            dto.ProductId = ProductId;
            if (IconFileAsset != null)
            {
                dto.IconAsset = IconFileAsset.ToDTO();
                dto.Icon = IconFileAsset.Url;
            }
            if (CharletAsset != null && CharletAsset.Count > 0)
                dto.Charlets = CharletAsset.Select(x => x.ToDTO()).ToList();
            if (StaticMeshAsset != null && StaticMeshAsset.Count > 0)
                dto.StaticMeshes = StaticMeshAsset.Select(x => x.ToDTO()).ToList();
            return dto;
        }
    }

    public class ProductSpecDTO : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public double Price { get; set; }
        public string TPID { get; set; }
        public string ProductId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public FileAssetDTO IconAsset { get; set; }
        public List<StaticMeshDTO> StaticMeshes { get; set; }
        public List<FileAssetDTO> Charlets { get; set; }
    }
}
