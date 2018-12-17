using Apps.Base.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Flurl;
using Flurl.Http;
using System.Threading.Tasks;

namespace Apps.Basic.Export.Services
{
    public class AccountMicroService : MicroServiceBase
    {

        public async Task<string> GetNameById(string id)
        {
            var name = await $"{Server}/Account".WithOAuthBearerToken(Token).GetStringAsync();
            return name;
        }
    }
}
