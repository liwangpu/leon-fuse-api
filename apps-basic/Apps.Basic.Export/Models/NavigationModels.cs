using System.ComponentModel.DataAnnotations;

namespace Apps.Basic.Export.Models
{
    /// <summary>
    /// 导航栏项创建模型
    /// </summary>
    public class NavigationCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Permission { get; set; }
        public string NodeType { get; set; }
        public string PagedModel { get; set; }
        public string Resource { get; set; }
        public string Field { get; set; }
        public string QueryParams { get; set; }
    }

    /// <summary>
    /// 导航栏项编辑模型
    /// </summary>
    public class NavigationEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Permission { get; set; }
        public string NodeType { get; set; }
        public string PagedModel { get; set; }
        public string Resource { get; set; }
        public string Field { get; set; }
        public string QueryParams { get; set; }
    }

    public class UserNavCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Role { get; set; }
        public string Description { get; set; }
    }

    public class UserNavEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Role { get; set; }
        public string Description { get; set; }
    }
}
