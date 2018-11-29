using System.Collections.Generic;

namespace Apps.Basic.Data.Entities
{
    public class UserNav
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public List<UserNavDetail> UserNavDetails { get; set; }
    }
}
