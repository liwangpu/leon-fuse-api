using Apps.Base.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Apps.Base.Common
{
    /// <summary>
    /// 对IEnumerable的分页查询的扩展
    /// </summary>
    public static class PagingFilterSearchExtention
    {
        /// <summary>
        /// 简单分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<PagedData<T>> SimplePaging<T>(this IQueryable<T> src, int page, int pageSize, Expression<Func<T, T>> expression = null) where T : class, new()
        {
            var res = new PagedData<T>();
            IQueryable<T> data = src;


            if (page < 1)
                page = 1;
            if (pageSize < 1)
                pageSize = 10;

            res.Total = await data.CountAsync();
            res.Page = page;

            if (((page - 1) * pageSize) > res.Total)
            {
                res.Data = new List<T>();
                res.Size = 0;
            }
            else
            {
                if (expression == null)
                    data = data.Skip((page - 1) * pageSize).Take(pageSize).Select(x => x);
                else
                    data = data.Skip((page - 1) * pageSize).Take(pageSize).Select(expression);
                res.Data = await data.ToListAsync();
                res.Size = res.Data.Count;
            }
            return res;
        }

    }
}


