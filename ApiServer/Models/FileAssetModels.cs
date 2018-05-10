using ApiModel.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.Models
{
    #region FileAssetCreateModel 资源文件创建模型
    /// <summary>
    /// 资源文件创建模型
    /// </summary>
    public class FileAssetCreateModel
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
        public string UploadTime { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string Icon { get; set; }
    }
    #endregion

    #region FileAssetEditModel 资源文件编辑模型
    /// <summary>
    /// 资源文件编辑模型
    /// </summary>
    public class FileAssetEditModel
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
        public string UploadTime { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string Icon { get; set; }
    }
    #endregion

    public class IconModel
    {
        public string ObjId { get; set; }
        public string AssetId { get; set; }
    }

    #region StaticMeshCreateModel 模型文件创建模型
    /// <summary>
    /// 模型文件创建模型
    /// </summary>
    public class StaticMeshCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string FileAssetId { get; set; }
    }
    #endregion

    #region StaticMeshEditModel 模型文件编辑模型
    /// <summary>
    /// 模型文件编辑模型
    /// </summary>
    public class StaticMeshEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string FileAssetId { get; set; }
    }
    #endregion


    #region StaticMeshUploadModel 规格模型上传模型
    /// <summary>
    /// 规格模型上传模型
    /// </summary>
    public class SpecStaticMeshUploadModel
    {
        public string ProductSpecId { get; set; }
        public string AssetId { get; set; }
    }
    #endregion

    #region SpecMaterialUploadModel 规格材料上传模型
    /// <summary>
    /// 规格材料上传模型
    /// </summary>
    public class SpecMaterialUploadModel
    {
        public string ProductSpecId { get; set; }
        public string StaticMeshId { get; set; }
        public string AssetId { get; set; }
    }
    #endregion

}
