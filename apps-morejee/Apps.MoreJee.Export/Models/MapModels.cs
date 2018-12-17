using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Apps.MoreJee.Export.Models
{
    #region MapCreateModel 地图创建模型
    /// <summary>
    /// 地图创建模型
    /// </summary>
    public class MapCreateModel 
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string PackageName { get; set; }
        public string UnCookedAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string IconAssetId { get; set; }
        public string FileAssetId { get; set; }

    }
    #endregion

    #region MapEditModel 地图编辑模型
    /// <summary>
    /// 地图编辑模型
    /// </summary>
    public class MapEditModel 
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string PackageName { get; set; }
        public string UnCookedAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string IconAssetId { get; set; }
        public string FileAssetId { get; set; }
    }
    #endregion
}
