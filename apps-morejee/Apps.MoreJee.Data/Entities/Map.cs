using Apps.Base.Common.Interfaces;
using System;

namespace Apps.MoreJee.Data.Entities
{
    public class Map : IEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        public string Modifier { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
        /// <summary>
        /// 数据激活状态标记
        /// </summary>
        public int ActiveFlag { get; set; }
        /// <summary>
        /// 场景引用资源文件Id
        /// </summary>
        public string FileAssetId { get; set; }
        /// <summary>
        /// 场景依赖项
        /// </summary>
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string PackageName { get; set; }
        public string UnCookedAssetId { get; set; }
        public string Icon { get; set; }
    }
}
