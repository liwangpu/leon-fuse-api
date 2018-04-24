using System.Collections.Generic;

namespace ApiModel.Entities
{
    public class FileAsset : EntityBase, IAsset
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
        public string Description { get; set; }
        public string Icon { get; set; }


        public override Dictionary<string, object> ToDictionary()
        {
            var dicData = new Dictionary<string, object>();
            dicData["Id"] = Id;
            dicData["Name"] = Name;
            dicData["CreatedTime"] = CreatedTime;
            dicData["ModifiedTime"] = ModifiedTime;
            dicData["Url"] = Url;
            dicData["Size"] = Size;
            dicData["FileExt"] = FileExt;
            dicData["LocalPath"] = LocalPath;
            dicData["Description"] = Description;
            dicData["UploadTime"] = UploadTime;
            return dicData;
        }
    }
}
