using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Basic.Export.Models
{
    #region TokenRequestModel Token请求模型
    /// <summary>
    /// Token请求模型
    /// </summary>
    public class TokenRequestModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Account { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Password { get; set; }
    }
    #endregion

    #region AccountCreateModel 用户创建模型
    /// <summary>
    /// 用户创建模型
    /// </summary>
    public class AccountCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public string DepartmentId { get; set; }
        public string Description { get; set; }
    }
    #endregion

    #region AccountEditModel 用户编辑模型
    /// <summary>
    /// 用户编辑模型
    /// </summary>
    public class AccountEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public string DepartmentId { get; set; }
        public string Description { get; set; }
    }
    #endregion

    #region ResetPasswordModel 重置密码编辑模型
    /// <summary>
    /// 重置密码编辑模型
    /// </summary>
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string UserId { get; set; }
        [StringLength(50, MinimumLength = 6, ErrorMessage = "长度必须大于6个字符")]
        public string Password { get; set; }
    }
    #endregion

    /// <summary>
    /// 部门创建模型
    /// </summary>
    public class DepartmentCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 部门编辑模型
    /// </summary>
    public class DepartmentUpdateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
