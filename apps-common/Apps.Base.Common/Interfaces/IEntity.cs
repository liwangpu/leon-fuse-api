using System;

namespace Apps.Base.Common.Interfaces
{
    public interface IEntity : IData
    {
        string Name { get; set; }
        string Creator { get; set; }
        string Modifier { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime ModifiedTime { get; set; }
    }
}
