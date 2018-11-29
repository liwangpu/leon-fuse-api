using Apps.Base.Common.Interfaces;
using System;
namespace Apps.Basic.Data.Entities
{
    public class Account : IEntity
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
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮件
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }
        /// <summary>
        /// 组织Id
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// 组织
        /// </summary>
        public Organization Organization { get; set; }
        /// <summary>
        /// 系统内置的基础角色Id
        /// </summary>
        public string InnerRoleId { get; set; }
        /// <summary>
        /// 系统内置的基础角色
        /// </summary>
        public UserRole InnerRole { get; set; }
    }
}
