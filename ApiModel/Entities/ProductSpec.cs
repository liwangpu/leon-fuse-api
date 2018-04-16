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
        [JsonIgnore]
        public List<StaticMesh> StaticMeshes { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        public string Charlets { get; set; }
        [NotMapped]
        public List<FileAsset> CharletAsset { get; set; }

        public ProductSpec()
        {
            CharletAsset = new List<FileAsset>();
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

            if (StaticMeshes != null && StaticMeshes.Count > 0)
            {
                dicData["StaticMeshes"] = StaticMeshes.Select(x => x.ToDictionary());
            }
            if (IconFileAsset != null)
            {
                dicData["Icon"] = IconFileAsset.ToDictionary();
            }
            if (CharletAsset != null && CharletAsset.Count > 0)
            {
                dicData["Charlets"] = CharletAsset.Select(x => x.ToDictionary());
            }
            return dicData;
        }
    }
}
