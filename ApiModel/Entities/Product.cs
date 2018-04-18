using System;
using System.Collections.Generic;
using System.Linq;
namespace ApiModel.Entities
{
    public class Product : EntityBase, IAsset
    {
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public List<ProductSpec> Specifications { get; set; }

        public override Dictionary<string, object> ToDictionary()
        {
            //var dicData = base.ToDictionary();

            var dicData = new Dictionary<string, object>();
            dicData["Id"] = Id;
            dicData["Name"] = Name;
            dicData["CreatedTime"] = CreatedTime;
            dicData["ModifiedTime"] = ModifiedTime;
            dicData["Description"] = Description;
            dicData["Icon"] = Icon;
            dicData["FolderId"] = FolderId;
            dicData["CategoryId"] = CategoryId;
            dicData["AccountId"] = AccountId;

            if (Specifications != null && Specifications.Count > 0)
            {
                dicData["Specifications"] = Specifications.Select(x => x.ToDictionary());
            }
            return dicData;
        }
    }

}
