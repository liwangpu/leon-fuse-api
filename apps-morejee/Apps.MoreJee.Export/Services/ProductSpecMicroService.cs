using Apps.Base.Common;
using Apps.MoreJee.Export.DTOs;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.MoreJee.Export.Services
{
    public class ProductSpecMicroService : MicroServiceBase
    {
        #region 构造函数
        public ProductSpecMicroService(string server, string token)
            : base(server, token)
        {

        }
        #endregion

        #region GetById 根据Id获取规格信息
        /// <summary>
        /// 根据Id获取规格信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductSpecDTO> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            var dto = await $"{Server}/ProductSpec/{id}".WithOAuthBearerToken(Token).AllowAnyHttpStatus().GetJsonAsync<ProductSpecDTO>();
            return dto;
        }
        #endregion
    }
}
