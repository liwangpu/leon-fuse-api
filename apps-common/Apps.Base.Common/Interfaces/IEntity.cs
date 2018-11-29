using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Base.Common.Interfaces
{
    public interface IEntity
    {
        string Id { get; set; }
        string Name { get; set; }
        string Creator { get; set; }
        string Modifier { get; set; }
        DateTime CreatedTime { get; set; }
        DateTime ModifiedTime { get; set; }
        int ActiveFlag { get; set; }
    }
}
