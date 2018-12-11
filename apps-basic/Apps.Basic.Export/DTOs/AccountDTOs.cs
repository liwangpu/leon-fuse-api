using System;

namespace Apps.Basic.Export.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public string Expires { get; set; }
    }

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
        public string Location { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Icon { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public string Description { get; set; }
    }

    public class DepartmentDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string OrganizationId { get; set; }
    }

}
