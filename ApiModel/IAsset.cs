using ApiModel.Entities;
using Newtonsoft.Json;

namespace ApiModel
{
    public interface IAsset : IListable
    {
        string FolderId { get; set; }
        string CategoryId { get; set; }
        string AccountId { get; set; }
        [JsonIgnore]
        Account Account { get; set; }
    }
}
