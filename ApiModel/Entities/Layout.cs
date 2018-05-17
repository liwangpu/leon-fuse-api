using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class Layout : EntityBase, IAsset
    {
        public string CategoryId { get; set; }
        public string Icon { get; set; }
        [NotMapped]
        public FileAsset IconFileAsset { get; set; }
        /// <summary>
        /// 户型数据,内容为类LayoutData的Json字符串。
        /// </summary>
        public string Data { get; set; }
    }
}
