using ApiModel;
using System.Collections.Generic;

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

        /// <summary>
        /// 明确指定过滤参数,在q语句完全支持前的过渡
        /// </summary>
        public List<QueryFilter> explicitFilters = new List<QueryFilter>();

        /// <summary>
        /// 从Q解析过滤参数
        /// </summary>
        /// <returns></returns>
        public List<QueryFilter> Filters()
        {
            var filters = new List<QueryFilter>();
            if (!string.IsNullOrWhiteSpace(Q))
            {

            }
            return filters;
        }
    }
}
