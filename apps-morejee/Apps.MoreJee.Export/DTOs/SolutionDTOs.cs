﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.MoreJee.Export.DTOs
{
    public class SolutionDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public string CreatorName { get; set; }
        public string ModifierName { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int ActiveFlag { get; set; }
        public string Description { get; set; }
        public string OrganizationId { get; set; }
        public string Icon { get; set; }
        public bool IsSnapshot { get; set; }
        public string SnapshotData { get; set; }
        public string LayoutId { get; set; }
        public string Data { get; set; }
    }
}