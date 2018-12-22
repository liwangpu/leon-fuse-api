using System;

namespace Apps.MoreJee.Export.DTOs
{
    public class ProductGroupDTO
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
        public string PivotLocation { get; set; }
        public int PivotType { get; set; }
        public int Orientation { get; set; }
        public string Items { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
