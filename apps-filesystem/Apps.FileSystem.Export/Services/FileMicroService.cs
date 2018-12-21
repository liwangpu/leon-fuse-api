using Apps.Base.Common;
using Apps.FileSystem.Export.DTOs;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace Apps.FileSystem.Export.Services
{
    public class FileMicroService : MicroServiceBase
    {

        #region 构造函数
        public FileMicroService(string server, string token)
            : base(server, token)
        {

        }
        #endregion

        #region GetById 根据Id获取文件信息
        /// <summary>
        /// 根据Id获取文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FileDTO> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;
            var dto = await $"{Server}/File/{id}".WithOAuthBearerToken(Token).AllowAnyHttpStatus().GetJsonAsync<FileDTO>();
            return dto;
        }
        #endregion

        #region GetUrlById 获取id对应文件url
        /// <summary>
        /// 获取id对应文件url
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public async Task GetUrlById(string id, Action<string> assign)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;
            var dto = await GetById(id);
            if (dto != null)
                assign(dto.Url);
        }
        #endregion

    }
}
