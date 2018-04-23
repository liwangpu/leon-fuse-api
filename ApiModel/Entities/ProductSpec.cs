using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
namespace ApiModel.Entities
{
    public class ProductSpec : EntityBase, IListable
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string CharletIds { get; set; }
        public string StaticMeshIds { get; set; }

        /// <summary>
        /// 价格，单位为元
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 第三方ID，此产品在供应商自己的系统比如ERP的ID
        /// </summary>
        public string TPID { get; set; }

        /// <summary>
        /// 所在产品的ID
        /// </summary>
        public string ProductId { get; set; }
        [JsonIgnore]
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

        public override Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = Id;
            dicData["Name"] = Name;
            dicData["CreatedTime"] = CreatedTime;
            dicData["ModifiedTime"] = ModifiedTime;
            dicData["Description"] = Description;
            dicData["Price"] = Price;
            dicData["TPID"] = TPID;
            dicData["ProductId"] = ProductId;

            if (StaticMeshAsset != null && StaticMeshAsset.Count > 0)
            {
                dicData["StaticMeshes"] = StaticMeshAsset.Where(x => x != null).Select(x => x.ToDictionary());
            }
            if (IconFileAsset != null)
            {
                dicData["IconAsset"] = IconFileAsset.ToDictionary();
                dicData["Icon"] = IconFileAsset.Url;
            }
            if (CharletAsset != null && CharletAsset.Count > 0)
            {
                dicData["Charlets"] = CharletAsset.Where(x => x != null).Select(x => x.ToDictionary());
            }
            return dicData;
        }
    }

    public class ProductSpecDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
