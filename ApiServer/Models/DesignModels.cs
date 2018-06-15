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

    #region BulkChangeCategoryModel 批量修改分类模型
    /// <summary>
    /// 批量修改分类模型
    /// </summary>
    public class BulkChangeCategoryModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string Ids { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string CategoryId { get; set; }
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
    public class MaterialCreateModel : ClientAssetCreateModel
    {
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
    public class MaterialEditModel : ClientAssetEditModel
    {
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
        public string IconAssetId { get; set; }
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
        public string IconAssetId { get; set; }
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
        public string IconAssetId { get; set; }
        public DateTime StateTime { get; set; }
    }
    #endregion

    #region PackageEditModel 套餐编辑模型
    /// <summary>
    /// 套餐编辑模型
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
        public string IconAssetId { get; set; }
        public DateTime StateTime { get; set; }
    }
    #endregion

    #region PackageAreaTypeEditModel 套餐区域编辑模型
    /// <summary>
    /// 套餐区域编辑模型
    /// </summary>
    public class PackageAreaTypeEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string PackageId { get; set; }
        public string Id { get; set; }
        public string AreaTypeId { get; set; }
        public string AreaAlias { get; set; }
    }
    #endregion

    #region PackageAreaTypeDeleteModel 套餐区域删除模型
    /// <summary>
    /// 套餐区域删除模型
    /// </summary>
    public class PackageAreaTypeDeleteModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string PackageId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Id { get; set; }
    }
    #endregion

    #region PackageProductGroupCreateModel 套餐产品组创建模型
    /// <summary>
    /// 套餐产品组创建模型
    /// </summary>
    public class PackageProductGroupCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string PackageId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string AreaId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ProductGroupId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string Serie { get; set; }
    }
    #endregion

    public class PackageProductGroupDeleteModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string PackageId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string AreaId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ProductGroupId { get; set; }
    }

    public class PackageCategoryProductCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string PackageId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string AreaId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ProductCategoryId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ProductId { get; set; }
    }

    public class PackageCategoryProductDeleteModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string PackageId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string AreaId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public string ProductId { get; set; }
    }

    #region MapCreateModel 地图创建模型
    /// <summary>
    /// 地图创建模型
    /// </summary>
    public class MapCreateModel : ClientAssetCreateModel
    {
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
    public class MapEditModel : ClientAssetEditModel
    {
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string IconAssetId { get; set; }
        public string FileAssetId { get; set; }
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
    public class TextureCreateModel : ClientAssetCreateModel
    {
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
    }
    #endregion

    #region TextureEditModel 贴图编辑模型
    /// <summary>
    /// 贴图编辑模型
    /// </summary>
    public class TextureEditModel : ClientAssetEditModel
    {
        public string IconAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
    }
    #endregion

    #region MediaCreateModel 媒体文件创建模型
    /// <summary>
    /// 媒体文件创建模型
    /// </summary>
    public class MediaCreateModel : EntityCreateModel
    {
        public string FileAssetId { get; set; }
        public string IconAssetId { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string SolutionId { get; set; }
        public string Type { get; set; }
    }
    #endregion

    #region MediaEditModel 媒体文件编辑模型
    /// <summary>
    /// 媒体文件编辑模型
    /// </summary>
    public class MediaEditModel : EntityEditModel
    {
        public string FileAssetId { get; set; }
        public string IconAssetId { get; set; }
        public string Rotation { get; set; }
        public string Location { get; set; }
        public string SolutionId { get; set; }
        public string Type { get; set; }
    }
    #endregion

    #region MediaShareResourceCreateModel 媒体分享信息创建模型
    /// <summary>
    /// 媒体分享信息创建模型
    /// </summary>
    public class MediaShareResourceCreateModel : EntityCreateModel
    {
        [Required(ErrorMessage = "必填信息")]
        public string MediaId { get; set; }
        [Required(ErrorMessage = "必填信息")]
        public long StartShareTimeStamp { get; set; }
        public long StopShareTimeStamp { get; set; }
        public string Password { get; set; }
    }
    #endregion

    #region MediaShareResourceEditModel 媒体分享信息编辑模型
    /// <summary>
    /// 媒体分享信息编辑模型
    /// </summary>
    public class MediaShareResourceEditModel : EntityEditModel
    {
        [Required(ErrorMessage = "必填信息")]
        public long StartShareTimeStamp { get; set; }
        public long StopShareTimeStamp { get; set; }
        public string Password { get; set; }
    }
    #endregion

    #region MediaShareRequestModel 共享资源文件查看请求模型
    /// <summary>
    /// 共享资源文件查看请求模型
    /// </summary>
    public class MediaShareRequestModel
    {
        public string Id { get; set; }
        public string MediaType { get; set; }
        public string Password { get; set; }
    }
    #endregion

    #region AreaTypeCreateModel 区域类型创建模型
    /// <summary>
    /// 区域类型创建模型
    /// </summary>
    public class AreaTypeCreateModel : EntityCreateModel
    {
        public string IconAssetId { get; set; }
    }
    #endregion

    #region AreaTypeEditModel 区域类型编辑模型
    /// <summary>
    /// 区域类型编辑模型
    /// </summary>
    public class AreaTypeEditModel : EntityEditModel
    {
        public string IconAssetId { get; set; }
    }
    #endregion


    public class ProductGroupCreateModel : EntityCreateModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Serie { get; set; }
        public string IconAssetId { get; set; }
        public string Items { get; set; }
        public string PivotLocation { get; set; }
        public int PivotType { get; set; }
        public int Orientation { get; set; }
    }

    public class ProductGroupEditModel : EntityEditModel
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "长度必须为1-50个字符")]
        public string Serie { get; set; }
        public string IconAssetId { get; set; }
        public string Items { get; set; }
        public string PivotLocation { get; set; }
        public int PivotType { get; set; }
        public int Orientation { get; set; }
    }


}
