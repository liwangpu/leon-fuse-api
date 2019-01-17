using Apps.Base.Common;
using Apps.MoreJee.Export.DTOs;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace Apps.MoreJee.Export.Services
{
    public class ProductMicroService : MicroServiceBase
    {
        #region 构造函数
        public ProductMicroService(string server, string token)
            : base(server, token)
        {

        }
        #endregion

        #region GetById 根据Id获取产品信息
        /// <summary>
        /// 根据Id获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductDTO> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            var dto = await $"{Server}/Products/{id}".WithOAuthBearerToken(Token).AllowAnyHttpStatus().GetJsonAsync<ProductDTO>();
            return dto;
        }
        #endregion

        public async Task GetById(string id, Action<ProductDTO> assign)
        {
            var dto = await GetById(id);
            if (dto != null)
                assign(dto);
        }
    }
}
