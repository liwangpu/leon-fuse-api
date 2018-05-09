using System;

namespace ApiModel
{
    public interface IEntity : IData
    {
        DateTime CreatedTime { get; set; }
        DateTime ModifiedTime { get; set; }
        string Creator { get; set; }
        string Modifier { get; set; }
        int ActiveFlag { get; set; }
    }
}
