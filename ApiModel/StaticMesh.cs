using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    public class StaticMesh : Asset
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

        /// <summary>
        /// 原始文件路径
        /// </summary>
        public string SrcFileUrl { get; set; }

        public List<Material> Materials { get; set; }

        public string ProductSpecId { get; set; }

        public ProductSpec ProductSpec { get; set; }

    }
}
