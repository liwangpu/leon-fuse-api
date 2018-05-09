using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class Layout : EntityBase, IAsset
    {
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public string Icon { get; set; }
        /// <summary>
        /// 户型数据,内容为类LayoutData的Json字符串。
        /// </summary>
        public string Data { get; set; }
    }
}
