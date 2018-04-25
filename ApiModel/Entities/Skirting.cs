using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    /// <summary>
    /// 墙上条状装饰覆盖物，由一个横截面拉长而成的物体，比如踢脚线，顶角线，腰线等。
    /// 轴心在左下角，靠墙的顶角上。
    /// </summary>
    public class Skirting : EntityBase, IAsset
    {
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
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
