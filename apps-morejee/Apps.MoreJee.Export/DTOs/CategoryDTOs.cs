using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.MoreJee.Export.DTOs
{
    public class CategoryDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string CreatorName { get; set; }
        public string ModifierName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ParentId { get; set; }
        public string OrganizationId { get; set; }
        public string Type { get; set; }
        public bool IsRoot { get; set; }
        public int DisplayIndex { get; set; }
        public List<CategoryDTO> Children { get; set; }
    }

    public class AssetCategoryPack
    {
        public List<CategoryDTO> Categories { get; set; }
    }
}
