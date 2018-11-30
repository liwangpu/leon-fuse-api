using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Basic.Export.DTOs
{
    public class AccountDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
    }

    public class AccountProfileDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Location { get; set; }
        public string Brief { get; set; }
        public string OrganizationId { get; set; }
        public string Organization { get; set; }
        public string DepartmentId { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
    }
}
