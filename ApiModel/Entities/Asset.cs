using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class Asset : EntityBase, IAsset
    {
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }
        public string Icon { get; set; }
    }
}
