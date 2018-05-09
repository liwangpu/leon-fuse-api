using ApiModel.Entities;
using BambooCore;
using System;

namespace ApiServer.Models
{
    #region AccountEditModel 用户信息编辑模型
    /// <summary>
    /// 用户信息编辑模型
    /// </summary>
    public class AccountEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool Frozened { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
        public string Type { get; set; }
        public DateTime? ExpireTime { get; set; }
        public DateTime? ActivationTime { get; set; }
    }
    #endregion

    #region RegisterAccountModel 用户注册模型
    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class RegisterAccountModel
    {
        public string Mail { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
        public DateTime? ExpireTime { get; set; }
        public DateTime? ActivationTime { get; set; }
    }
    #endregion

    public class ResetPasswordModel
    {
        public string Email { get; set; }
    }

    public class NewPasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AccountProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public string Location { get; set; }
        public string Brief { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
    }

    public class OrganizationEditModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Mail { get; set; }
        public string Location { get; set; }
        public string ParentId { get; set; }
        public string OwnerId { get; set; }
    }

}
