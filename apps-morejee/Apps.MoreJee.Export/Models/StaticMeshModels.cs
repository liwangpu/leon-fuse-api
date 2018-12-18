using System.ComponentModel.DataAnnotations;

namespace Apps.MoreJee.Export.Models
{
    public class StaticMeshCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string PackageName { get; set; }
        public string UnCookedAssetId { get; set; }
        public string FileAssetId { get; set; }
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string SrcFileAssetId { get; set; }
        public string Materials { get; set; }
    }

    public class StaticMeshUpdateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string PackageName { get; set; }
        public string UnCookedAssetId { get; set; }
        public string FileAssetId { get; set; }
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string SrcFileAssetId { get; set; }
        public string Materials { get; set; }
    }
}
