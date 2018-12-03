using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Basic.Export.DTOs
{
    public class UserNavigationDTO
    {
        public string Id { get; set; }
        public string ExcludeFiled { get; set; }
        public string ExcludePermission { get; set; }
        public string ExcludeQueryParams { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string NodeType { get; set; }
        public string Icon { get; set; }
        public string Resource { get; set; }
        public string Permission { get; set; }
        public string PagedModel { get; set; }
        public string Field { get; set; }
        public string QueryParams { get; set; }
        public bool NewTapOpen { get; set; }
        public int Grade { get; set; }
        public string ParentId { get; set; }
    }

    public class NavigationDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string NodeType { get; set; }
        public string Icon { get; set; }
        public string Resource { get; set; }
        public string Permission { get; set; }
        public string PagedModel { get; set; }
        public string Field { get; set; }
        public string QueryParams { get; set; }
        public bool NewTapOpen { get; set; }
        public int Grade { get; set; }
        public string ParentId { get; set; }
    }

    public class UserNavDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public List<UserNavigationDTO> UserNavDetails { get; set; }
    }
}
