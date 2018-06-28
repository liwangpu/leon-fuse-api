using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.Services
{
    public class AppConfig
    {
        private IConfiguration _AppCfg;
        private static AppConfig _instance = new AppConfig();
        public static AppConfig Instance { get { return _instance; } }
        AppConfig()
        {

        }

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="cfg"></param>
        public void Init(IConfiguration cfg)
        {
            _AppCfg = cfg;
        }
        #endregion

        /// <summary>
        /// 媒体分享文件服务器
        /// </summary>
        public string MediaShareServer
        {
            get
            {
                return _AppCfg["MediaShareServer"];
            }
        }
    }
}
