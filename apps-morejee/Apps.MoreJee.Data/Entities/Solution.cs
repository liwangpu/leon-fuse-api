﻿using Apps.Base.Common.Interfaces;
using System;


namespace Apps.MoreJee.Data.Entities
{
    public class Solution : IEntity, IListView
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
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 组织Id
        /// </summary>
        public string OrganizationId { get; set; }
        public string Icon { get; set; }
        public bool IsSnapshot { get; set; }
        public string SnapshotData { get; set; }
        public string LayoutId { get; set; }
        public Layout Layout { get; set; }
        public string Data { get; set; }
    }
}