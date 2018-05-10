namespace ApiServer.Models
{
    public class PagingRequestModel
    {
        public string Q { get; set; }
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool Desc { get; set; }
    }
}
