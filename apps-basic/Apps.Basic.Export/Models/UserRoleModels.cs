using System;
using System.ComponentModel.DataAnnotations;

namespace Apps.Basic.Export.Models
{
    /// <summary>
    /// 用户角色创建模型
    /// </summary>
    public class UserRoleCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 用户角色编辑模型
    /// </summary>
    public class UserRoleEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        public string Description { get; set; }
    }




}
