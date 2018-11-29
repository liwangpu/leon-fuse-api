using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Basic.Data.Entities
{
    public class UserRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string KeyWord { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}
