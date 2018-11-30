using Apps.Base.Common.Interfaces;

namespace Apps.Basic.Data.Entities
{
    public class Navigation : IData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string Permission { get; set; }
        public string PagedModel { get; set; }
        public string Resource { get; set; }
        public string Field { get; set; }
        public string NodeType { get; set; }
        public bool IsInner { get; set; }
        public bool NewTapOpen { get; set; }
        public string QueryParams { get; set; }
    }
}
