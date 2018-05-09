using System;

namespace ApiModel
{
    /// <summary>
    /// 列表类型资源接口
    /// </summary>
    public interface IListable : IData
    {
        string Description { get; set; }
        string Icon { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime ModifiedTime { get; set; }
    }
}
