using System.ComponentModel.DataAnnotations;

namespace Apps.MoreJee.Export.Models
{
    #region MapCreateModel 产品组创建模型
    /// <summary>
    /// 产品组创建模型
    /// </summary>
    public class ProductGroupCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string IconAssetId { get; set; }
        public string PivotLocation { get; set; }
        public int PivotType { get; set; }
        public int Orientation { get; set; }
        public string Items { get; set; }
    }
    #endregion

    #region MapEditModel 产品组编辑模型
    /// <summary>
    /// 产品组编辑模型
    /// </summary>
    public class ProductGroupUpdateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string IconAssetId { get; set; }
        public string PivotLocation { get; set; }
        public int PivotType { get; set; }
        public int Orientation { get; set; }
        public string Items { get; set; }
    }
    #endregion
}
