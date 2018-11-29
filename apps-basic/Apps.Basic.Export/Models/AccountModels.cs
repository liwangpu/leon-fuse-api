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
        public string Password { get; set; }
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
        public string Password { get; set; }
    }
    #endregion
}
