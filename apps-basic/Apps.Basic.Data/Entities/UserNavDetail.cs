using System.ComponentModel.DataAnnotations.Schema;

namespace Apps.Basic.Data.Entities
{
    public class UserNavDetail
    {
        public string Id { get; set; }
        public string ExcludeFiled { get; set; }
        public string ExcludePermission { get; set; }
        public string ExcludeQueryParams { get; set; }
        public int Grade { get; set; }
        public string ParentId { get; set; }
        public string RefNavigationId { get; set; }
        public string UserNavId { get; set; }
        public UserNav UserNav { get; set; }
    }
}
