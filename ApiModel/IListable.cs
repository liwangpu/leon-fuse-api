using System;

namespace ApiModel
{
    public interface IListable : IEntity
    {
        string Description { get; set; }
        string Icon { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime ModifiedTime { get; set; }
    }
}
