using Apps.Base.Common.Interfaces;
using System;
namespace Apps.Basic.Data.Entities
{
    public class Account :IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
    }
}
