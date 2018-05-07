using System;

namespace ApiModel
{
    public interface IListable : IData
    {
        string Description { get; set; }
        string Icon { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime ModifiedTime { get; set; }
    }
}
