using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel
{
    public class StaticMesh : Asset
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public FileAsset File { get; set; }

        /// <summary>
        /// 原始文件路径
        /// </summary>
        public string SrcFileUrl { get; set; }

        /// <summary>
        /// 模型的一些额外数据，比如面数，UV数，是否有碰撞等， key-value对。
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 使用的材质
        /// </summary>
        public List<Material> Materials { get; set; }

        /// <summary>
        /// 依赖的资源文件
        /// </summary>
        public List<FileAsset> Dependencies { get; set; }
    }
}
