using System;
using System.Collections.Generic;
using System.Text;

namespace Apps.Base.Common.Models
{
    /// <summary>
    /// 通用的分页请求模型
    /// </summary>
    public class PagingRequestModel
    {
        public string Search { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool? Desc { get; set; }
    }
}
