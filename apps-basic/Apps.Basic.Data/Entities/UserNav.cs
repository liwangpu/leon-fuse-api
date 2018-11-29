using System.Collections.Generic;

namespace Apps.Basic.Data.Entities
{
    public class UserNav
    {
        public string Role { get; set; }
        public List<UserNavDetail> UserNavDetails { get; set; }
    }
}
