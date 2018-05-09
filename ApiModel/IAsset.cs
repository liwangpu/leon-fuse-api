using ApiModel.Entities;
using Newtonsoft.Json;

namespace ApiModel
{
    /// <summary>
    /// 文件资源类型接口
    /// </summary>
    public interface IAsset : IListable
    {
        string FolderId { get; set; }
        string CategoryId { get; set; }
        string AccountId { get; set; }
        Account Account { get; set; }
    }
}
