using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class AssetTag : EntityBase, IListable
    {
        public string Icon { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
    }
}
