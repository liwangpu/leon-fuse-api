using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    public class Material : Asset
    {
        /// <summary>
        /// 原始文件路径
        /// </summary>
        public FileAsset File { get; set; }

        /// <summary>
        /// 材质参数， key-value对。
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 依赖的资源文件
        /// </summary>
        public List<FileAsset> Dependencies { get; set; }
    }
}
