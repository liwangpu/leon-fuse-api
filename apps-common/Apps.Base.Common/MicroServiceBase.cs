using System;
using System.Collections.Generic;
using System.Text;
using Flurl;
using Flurl.Http;



namespace Apps.Base.Common
{
    public class MicroServiceBase
    {
        public  string Token { get; }
        public string Server { get ; }

        #region 构造函数
        public MicroServiceBase(string server, string token)
        {
            Server = server;
            Token = token;
        } 
        #endregion

    }
}
