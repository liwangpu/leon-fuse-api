﻿using ApiModel.Entities;
using ApiServer.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace ApiServer.Stores
{
    public class MapStore : ListableStore<Map, MapDTO>, IStore<Map, MapDTO>
    {
        #region 构造函数
        public MapStore(ApiDbContext context)
        : base(context)
        { }
        #endregion

        #region SatisfyCreateAsync 判断数据是否满足存储规范
        /// <summary>
        /// 判断数据是否满足存储规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyCreateAsync(string accid, Map data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion

        #region SatisfyUpdateAsync 判断数据是否满足更新规范
        /// <summary>
        /// 判断数据是否满足更新规范
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="data"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task SatisfyUpdateAsync(string accid, Map data, ModelStateDictionary modelState)
        {
            await Task.FromResult(string.Empty);
        }
        #endregion
    }
}