using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    /// <summary>
    /// 产品规格信息
    /// </summary>
    public class ProductSpec : EntityBase, IListable, IDTOTransfer<IData>
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 贴图ids(逗号分隔的FileAssetId)
        /// </summary>
        public string CharletIds { get; set; }
        /// <summary>
        /// 模型材质依赖信息(格式为SpecMeshMap,请看下文)
        /// </summary>
        public string StaticMeshIds { get; set; }
        /// <summary>
        /// 价格，单位为元
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 第三方ID，此产品在供应商自己的系统比如ERP的ID
        /// </summary>
        public string TPID { get; set; }

        /// <summary>
        /// 所在产品的ID
        /// </summary>
        public string ProductId { get; set; }
        public Product Product { get; set; }

        /**************** DTO专用容器 ****************/
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

        public IData ToDTO()
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

    /// <summary>
    /// 产品规格信息DTO
    /// </summary>
    public class ProductSpecDTO : DataBase
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public decimal Price { get; set; }
        public string TPID { get; set; }
        public string ProductId { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public IData IconAsset { get; set; }
        public List<IData> StaticMeshes { get; set; }
        public List<IData> Charlets { get; set; }
    }

    /// <summary>
    /// 产品规格-模型和材质文件依赖,对应ProductSpec.StaticMeshIds(仅用来序列化反序列化字符串信息,非数据持久类)
    /// </summary>
    public class SpecMeshMap
    {
        public SpecMeshMap()
        {
            Items = new List<SpecMeshMapItem>();
        }
        public List<SpecMeshMapItem> Items { get; set; }
    }

    /// <summary>
    /// 产品规格-模型和材质文件依赖信息(仅用来序列化反序列化字符串信息,非数据持久类)
    /// </summary>
    public class SpecMeshMapItem
    {
        public SpecMeshMapItem()
        {
            MaterialIds = new List<string>();
        }

        /// <summary>
        /// 模型文件id
        /// </summary>
        public string StaticMeshId { get; set; }
        /// <summary>
        /// 所依赖材质文件Ids
        /// </summary>
        public List<string> MaterialIds { get; set; }
    }
}
