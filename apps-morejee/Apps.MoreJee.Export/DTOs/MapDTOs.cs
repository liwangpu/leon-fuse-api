using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.MoreJee.Export.DTOs
{
    public class MapDTO
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
        public string FileAssetId { get; set; }
        public string Dependencies { get; set; }
        public string Properties { get; set; }
        public string PackageName { get; set; }
        public string UnCookedAssetId { get; set; }
        public string Icon { get; set; }
    }
}
