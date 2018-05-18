using System;
using System.ComponentModel.DataAnnotations;

namespace ApiServer.Models
{
    #region ProductCreateModel 产品新建模型
    /// <summary>
    /// 产品新建模型
    /// </summary>
    public class ProductCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region ProductEditModel 产品编辑模型
    /// <summary>
    /// 产品编辑模型
    /// </summary>
    public class ProductEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region ProductSpecCreateModel 产品规格新建模型
    /// <summary>
    /// 产品规格新建模型
    /// </summary>
    public class ProductSpecCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "价格信息有误")]
        public decimal Price { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region ProductSpecEditModel 产品规格编辑模型
    /// <summary>
    /// 产品规格编辑模型
    /// </summary>
    public class ProductSpecEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "价格信息有误")]
        public decimal Price { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region ProductSpecAutoRelatedEditModel 根据规格自动创建或者关联产品规格编辑模型
    /// <summary>
    /// 根据规格自动创建或者关联产品规格编辑模型
    /// </summary>
    public class ProductSpecAutoRelatedEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string StaticMeshId { get; set; }
        public string ProductId { get; set; }
    }
    #endregion

    #region SolutionCreateModel 解决方案新建模型
    /// <summary>
    /// 解决方案新建模型
    /// </summary>
    public class SolutionCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Data { get; set; }
        public string CategoryId { get; set; }
        public string LayoutId { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region SolutionEditModel 解决方案编辑模型
    /// <summary>
    /// 解决方案编辑模型
    /// </summary>
    public class SolutionEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Data { get; set; }
        public string CategoryId { get; set; }
        public string LayoutId { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region MaterialCreateModel 材质创建模型
    /// <summary>
    /// 材质创建模型
    /// </summary>
    public class MaterialCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }
        public string CategoryId { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region MaterialEditModel 材质编辑模型
    /// <summary>
    /// 材质编辑模型
    /// </summary>
    public class MaterialEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Icon { get; set; }
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Parameters { get; set; }
        public string CategoryId { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region OrderCreateModel 订单创建模型
    /// <summary>
    /// 订单创建模型
    /// </summary>
    public class OrderCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Content { get; set; }
        public string State { get; set; }
        public DateTime StateTime { get; set; }
    }
    #endregion

    #region OrderEditModel 订单创建模型
    /// <summary>
    /// 订单创建模型
    /// </summary>
    public class OrderEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Content { get; set; }
        public string State { get; set; }
        public DateTime StateTime { get; set; }
    }
    #endregion

    #region PackageCreateModel 套餐创建模型
    /// <summary>
    /// 套餐创建模型
    /// </summary>
    public class PackageCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Content { get; set; }
        public string State { get; set; }
        public DateTime StateTime { get; set; }
    }
    #endregion

    #region PackageEditModel 订单编辑模型
    /// <summary>
    /// 订单编辑模型
    /// </summary>
    public class PackageEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string Content { get; set; }
        public string State { get; set; }
        public DateTime StateTime { get; set; }
    }
    #endregion

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
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string IconAssetId { get; set; }
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
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string IconAssetId { get; set; }
    }
    #endregion

    #region LayoutCreateModel 户型创建模型
    /// <summary>
    /// 户型创建模型
    /// </summary>
    public class LayoutCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string CategoryId { get; set; }
        public string Data { get; set; }
    }
    #endregion

    #region LayoutEditModel 户型编辑模型
    /// <summary>
    /// 户型编辑模型
    /// </summary>
    public class LayoutEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string CategoryId { get; set; }
        public string Data { get; set; }
    }
    #endregion


    #region TextureCreateModel 贴图创建模型
    /// <summary>
    /// 贴图创建模型
    /// </summary>
    public class TextureCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
    }
    #endregion

    #region TextureEditModel 贴图编辑模型
    /// <summary>
    /// 贴图编辑模型
    /// </summary>
    public class TextureEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Name { get; set; }
        [StringLength(200, ErrorMessage = "长度必须为0-200个字符")]
        public string Description { get; set; }
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
    }
    #endregion
}
