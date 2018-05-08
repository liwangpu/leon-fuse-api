using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class AssetTag : EntityBase, IListable
    {
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
