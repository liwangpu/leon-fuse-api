﻿using System;

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
        public string Location { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Icon { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
    }
}