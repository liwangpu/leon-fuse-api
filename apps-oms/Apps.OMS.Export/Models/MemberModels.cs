using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Apps.OMS.Export.Models
{
    /// <summary>
    /// 会员邀请创建模型
    /// </summary>
    public class MemberRegistryCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Company { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Province { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string City { get; set; }
        public string Area { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Inviter { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 会员邀请编辑模型
    /// </summary>
    public class MemberRegistryUpdateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Mail { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Company { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Province { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string City { get; set; }
        public string Area { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Inviter { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 会员邀请审批模型
    /// </summary>
    public class MemberRegistryApproveModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
    }
}
