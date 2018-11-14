using System.ComponentModel.DataAnnotations.Schema;

namespace ApiModel.Entities
{
    public class UserNavDetail
    {
        public string Id { get; set; }
        public string Permission { get; set; }
        public string PagedModel { get; set; }
        public string Field { get; set; }
        public int Grade { get; set; }
        public string ParentId { get; set; }
        public string RefNavigationId { get; set; }
        public UserNav UserNav { get; set; }

        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Title { get; set; }
        [NotMapped]
        public string Url { get; set; }
        [NotMapped]
        public string NodeType { get; set; }
        [NotMapped]
        public string Icon { get; set; }

        [NotMapped]
        public Navigation RefNavigation { get; set; }

    }
}
