using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class ClientAsset : EntityBase, IAsset
    {
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
