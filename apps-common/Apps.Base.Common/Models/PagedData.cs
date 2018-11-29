using System.Collections.Generic;

namespace Apps.Base.Common.Models
{
    /// <summary>
    /// 通用的分页数据结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedData<T>
    {
        public IList<T> Data { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
        public int Total { get; set; }
    }
}
