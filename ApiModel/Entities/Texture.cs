using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Texture : EntityBase, IListable
    {
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        [NotMapped]
        public FileAsset FileAsset { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }

    }
}
