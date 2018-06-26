using System.ComponentModel.DataAnnotations;

namespace ApiServer.Models
{
    #region AccountEditModel 用户信息编辑模型
    /// <summary>
    /// 用户信息编辑模型
    /// </summary>
    public class AccountEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string DepartmentId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ExpireTime { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ActivationTime { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Mail { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(50, ErrorMessage = "长度必须为0-50个字符")]
        public string Password { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public bool Frozened { get; set; }
        public bool IsAdmin { get; set; }
    }
    #endregion

    #region RegisterAccountModel 用户注册模型
    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class RegisterAccountModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Mail { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(50, ErrorMessage = "长度必须为0-50个字符")]
        public string Password { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public string OrganizationId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string DepartmentId { get; set; }
        //[Required(ErrorMessage = "必填信息")]
        public bool IsAdmin { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ExpireTime { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ActivationTime { get; set; }
    }
    #endregion

    public class NewPasswordModel
    {
        [StringLength(50, MinimumLength = 6, ErrorMessage = "长度必须大于6个字符")]
        public string OldPassword { get; set; }
        [StringLength(50, MinimumLength = 6, ErrorMessage = "长度必须大于6个字符")]
        public string NewPassword { get; set; }
    }

    #region AccountProfileModel 用户账户基础信息
    /// <summary>
    /// 用户账户基础信息
    /// </summary>
    public class AccountProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Location { get; set; }
        public string Brief { get; set; }
        public string OrganizationId { get; set; }
        public string DepartmentId { get; set; }
        public string Role { get; set; }
    }
    #endregion

    #region OrganizationCreateModel 组织新建模型
    /// <summary>
    /// 组织新建模型
    /// </summary>
    public class OrganizationCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        [StringLength(50, ErrorMessage = "长度必须为0-50个字符")]
        public string Mail { get; set; }
        [StringLength(50, ErrorMessage = "长度必须为0-50个字符")]
        public string Location { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region OrganizationEditModel 组织编辑模型
    /// <summary>
    /// 组织编辑模型
    /// </summary>
    public class OrganizationEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        [StringLength(50, ErrorMessage = "长度必须为0-50个字符")]
        public string Mail { get; set; }
        [StringLength(50, ErrorMessage = "长度必须为0-50个字符")]
        public string Location { get; set; }
        public string ParentId { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

}
