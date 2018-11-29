using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apps.Basic.Service
{
    public class AppConfig
    {
        public string ConnectionString { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public GuidSettings GuidSettings { get; set; }
    }

    /// <summary>
    /// Jwt配置
    /// </summary>
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpiresDay { get; set; }
    }

    public class GuidSettings
    {
        public int ServerId { get; set; }
        public string GuidSalt { get; set; }
        public int GuidMinLen { get; set; }
    }
}
