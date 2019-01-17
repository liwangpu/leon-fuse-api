using Apps.Base.Common;
using Apps.MoreJee.Export.DTOs;
using Flurl.Http;
using System;
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

        /// <summary>
        /// 根据Id获取规格信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public async Task GetById(string id, Action<ProductSpecDTO> assign)
        {
            var dto = await GetById(id);
            if (dto != null)
                assign(dto);
        }
        #endregion

        #region GetBriefById 根据Id获取产品规格简洁信息
        /// <summary>
        /// 根据Id获取产品规格简洁信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductSpecDTO> GetBriefById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            var dto = await $"{Server}/ProductSpec/Brief/{id}".AllowAnyHttpStatus().GetJsonAsync<ProductSpecDTO>();
            return dto;
        }

        /// <summary>
        /// GetBriefById
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public async Task GetBriefById(string id, Action<ProductSpecDTO> assign)
        {
            var dto = await GetBriefById(id);
            if (dto != null)
                assign(dto);
        }
        #endregion
    }
}
