using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace Apps.MoreJee.Export.Models
{
    public class CategoryQueryModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Type { get; set; }
    }

    public class CategoryCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Type { get; set; }
        public string ParentId { get; set; }
        public string IconAssetId { get; set; }
    }

    public class CategoryUpdateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string ParentId { get; set; }
        public string IconAssetId { get; set; }
    }
}
