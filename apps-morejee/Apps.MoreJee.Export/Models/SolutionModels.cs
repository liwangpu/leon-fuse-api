using System.ComponentModel.DataAnnotations;

namespace Apps.MoreJee.Export.Models
{
    public class SolutionCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string Data { get; set; }
        public bool IsSnapshot { get; set; }
        public string SnapshotData { get; set; }
        public string LayoutId { get; set; }
    }

    public class SolutionUpdateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string Data { get; set; }
        public bool IsSnapshot { get; set; }
        public string SnapshotData { get; set; }
        public string LayoutId { get; set; }
    }
}
