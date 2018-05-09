using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Map : EntityBase, IListable
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        [NotMapped]
        public FileAsset FileAsset { get; set; }

        public string Dependencies { get; set; }
        public string Properties { get; set; }
    }
}
