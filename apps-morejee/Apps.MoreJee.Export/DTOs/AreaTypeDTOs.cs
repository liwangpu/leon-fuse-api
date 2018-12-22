using System;

namespace Apps.MoreJee.Export.DTOs
{
    public class AreaTypeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string CreatorName { get; set; }
        public string ModifierName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Description { get; set; }
        public string OrganizationId { get; set; }
    }
}
