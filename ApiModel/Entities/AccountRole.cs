using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModel.Entities
{
    public class AccountRole
    {
        public string Id { get; set; }
        public Account Account { get; set; }
        public string UserRoleId { get; set; }
    }
}
