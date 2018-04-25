using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class AssetTag : EntityBase, IListable
    {
        public string Description { get; set; }
        public string Icon { get; set; }

        public override Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = this.Id;
            dicData["Name"] = this.Name;
            return dicData;
        }
    }
}
