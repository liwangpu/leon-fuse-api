using Apps.Base.Common.Interfaces;
using System;

namespace Apps.MoreJee.Data.Entities
{
    public class ProductReplaceGroup : IEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Modifier { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }
        public int ActiveFlag { get; set; }
        public string OrganizationId { get; set; }
        public string Description { get; set; }
        public string DefaultItemId { get; set; }
        public string GroupItemIds { get; set; }
    }
}
