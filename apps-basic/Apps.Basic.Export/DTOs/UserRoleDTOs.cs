using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Basic.Export.DTOs
{
    public class UserRoleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public bool IsInner { get; set; }
    }
}
