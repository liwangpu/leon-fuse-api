using Apps.Base.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace Apps.Basic.Data.Entities
{
    public class Organization : IEntity
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
        /// 失效时间
        /// </summary>
        public DateTime ExpireTime { get; set; }
        /// <summary>
        /// 启用时间
        /// </summary>
        public DateTime ActivationTime { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 组织超级管理员Id
        /// </summary>
        public string OwnerId { get; set; }
        /// <summary>
        /// 组织人员
        /// </summary>
        public List<Account> Accounts { get; set; }
        /// <summary>
        /// 组织类型id
        /// </summary>
        public string OrganizationTypeId { get; set; }
        /// <summary>
        /// 组织类型
        /// </summary>
        public OrganizationType OrganizationType { get; set; }
    }
}
