using ApiModel.Entities;

namespace ApiServer.Models
{
    public class FileAssetModel : IModel<FileAsset>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Md5 { get; set; }
        public long Size { get; set; }
        public string FileExt { get; set; }
        public string LocalPath { get; set; }
        public string UploadTime { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public FileAsset ToEntity()
        {
            var entity = new FileAsset();
            entity.Id = Id;
            entity.Name = Name;
            entity.Url = Url;
            entity.Md5 = Md5;
            entity.Size = Size;
            entity.FileExt = FileExt;
            entity.LocalPath = LocalPath;
            entity.UploadTime = UploadTime;
            entity.FolderId = FolderId;
            entity.CategoryId = CategoryId;
            entity.AccountId = AccountId;
            entity.Description = Description;
            entity.Icon = Icon;
            return entity;
        }
    }

    public class IconModel
    {
        public string ObjId { get; set; }
        public string AssetId { get; set; }
    }

    public class StaticMeshEditModel : IModel<StaticMesh>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FileAssetId { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public StaticMesh ToEntity()
        {
            var entity = new StaticMesh();
            entity.Id = Id;
            entity.Name = Name;
            entity.Description = Description;
            entity.FileAssetId = FileAssetId;
            entity.Icon = Icon;
            return entity;
        }
    }

    public class MaterialEditModel : IModel<Material>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FileAssetId { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public Material ToEntity()
        {
            var entity = new Material();
            entity.Id = Id;
            entity.Name = Name;
            entity.Description = Description;
            entity.Icon = FileAssetId;
            return entity;
        }
    }

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
