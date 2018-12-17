using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Apps.FileSystem.Export.Models
{
    #region FileAssetCreateModel 资源文件创建模型
    /// <summary>
    /// 资源文件创建模型
    /// </summary>
    public class FileCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Url { get; set; }
        public string Md5 { get; set; }
        public long Size { get; set; }
        public string FileExt { get; set; }
        public string LocalPath { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string Icon { get; set; }
        public int FileState { get; set; }
    }
    #endregion

    #region FileAssetEditModel 资源文件编辑模型
    /// <summary>
    /// 资源文件编辑模型
    /// </summary>
    public class FileEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Url { get; set; }
        public string Md5 { get; set; }
        public long Size { get; set; }
        public string FileExt { get; set; }
        public string LocalPath { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string Icon { get; set; }
        public int FileState { get; set; }
    }
    #endregion
}
