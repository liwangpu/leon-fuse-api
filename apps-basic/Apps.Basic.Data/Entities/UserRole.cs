﻿using Apps.Base.Common.Interfaces;
using System;

namespace Apps.Basic.Data.Entities
{
    public class UserRole : IEntity
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
        /// 角色关键词
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 是否内置量
        /// </summary>
        public bool IsInner { get; set; }
    }
}
