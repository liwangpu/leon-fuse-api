using System;
using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class FileAsset : EntityBase, IAsset, IDTOTransfer<FileAssetDTO>
    {
        /// <summary>
        /// 原始文件路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 原始文件md5
        /// </summary>
        public string Md5 { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// 扩展名，比如.jpg, .png, .fbx
        /// </summary>
        public string FileExt { get; set; }
        /// <summary>
        /// 原始文件上传时本地路径
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 资源上传时间
        /// </summary>
        public string UploadTime { get; set; }
        public string FolderId { get; set; }
        public string CategoryId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }
        public string Icon { get; set; }

        public FileAssetDTO ToDTO()
        {
            var dto = new FileAssetDTO();
            dto.Id = Id;
            dto.Name = Name;
            dto.Icon = Icon;
            dto.Description = Description;
            dto.Url = Url;
            dto.Md5 = Md5;
            dto.Size = Size;
            dto.FileExt = FileExt;
            dto.LocalPath = LocalPath;
            dto.UploadTime = UploadTime;
            dto.FolderId = FolderId;
            dto.CategoryId = CategoryId;
            dto.AccountId = AccountId;
            return dto;
        }
    }

    public class FileAssetDTO : DataBase
    {
        public string Icon { get; set; }
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
    }

}
