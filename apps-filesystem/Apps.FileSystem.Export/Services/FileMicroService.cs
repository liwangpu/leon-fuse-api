using Apps.Base.Common;
using Apps.FileSystem.Export.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace Apps.FileSystem.Export.Services
{
    public class FileMicroService: MicroServiceBase
    {

        #region 构造函数
        public FileMicroService(string server, string token)
            : base(server, token)
        {

        }
        #endregion

        public async Task<FileDTO> GetById(string id)
        {
            var dto = await $"{Server}/File".WithOAuthBearerToken(Token).GetJsonAsync<FileDTO>();

            return dto;
        }

    }
}
