using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Basic.Export.DTOs
{
    public class OrganizationDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string OrganizationTypeId { get; set; }
        public string OrganizationTypeName { get; set; }
    }

    public class OrganizationTypeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public DateTime ActivationTime { get; set; }
        public bool IsInner { get; set; }
    }

}
