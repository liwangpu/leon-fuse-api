using System;
using System.Collections.Generic;
using System.Text;
using Flurl;
using Flurl.Http;



namespace Apps.Base.Common
{
    public class MicroServiceBase
    {
        private string _Token;
        private string _Server;

        public string Token { get { return _Token; } }
        public string Server { get { return _Server; } }


        public void Init(string server, string token)
        {
            _Server = server;
            _Token = token;
        }
    }
}
