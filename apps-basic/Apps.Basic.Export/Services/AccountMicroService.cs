using Apps.Base.Common;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.Basic.Export.Services
{
    public class AccountMicroService : MicroServiceBase
    {

        #region 构造函数
        public AccountMicroService(string server, string token)
            : base(server, token)
        {

        }
        #endregion

        #region GetNameById 根据id获取用户姓名
        /// <summary>
        /// 根据id获取用户姓名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetNameById(string id)
        {
            var name = await $"{Server}/Account/GetNameById?id={id}".WithOAuthBearerToken(Token).AllowAnyHttpStatus().GetStringAsync();
            return name;
        }
        #endregion

        #region GetNameByIds 根据用户Ids获取用户姓名集合
        /// <summary>
        /// 根据用户Ids获取用户姓名集合
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<List<string>> GetNameByIds(string ids)
        {
            var names = await $"{Server}/Account/GetNameByIds?ids={ids}".WithOAuthBearerToken(Token).AllowAnyHttpStatus().GetJsonAsync<List<string>>();
            return names;
        }
        #endregion

        #region GetNameByIds 获取资源的创建人和更新人姓名
        /// <summary>
        /// 获取资源的创建人和更新人姓名
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="modifier"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public async Task GetNameByIds(string creator, string modifier, Action<string, string> assign)
        {
            creator = string.IsNullOrEmpty(creator) ? string.Empty : creator;
            modifier = string.IsNullOrEmpty(modifier) ? string.Empty : modifier;
            var ids = $"{creator},{modifier}";
            var names = await GetNameByIds(ids);
            assign(names[0], names[1]);
        } 
        #endregion

    }
}
