using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Base.Common.Interfaces
{
    public interface ITree
    {
        string Id { get; set; }
        int LValue { get; set; }
        int RValue { get; set; }
        string ParentId { get; set; }
        string NodeType { get; set; }
        string ObjId { get; set; }
    }
}
