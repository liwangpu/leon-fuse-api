namespace ApiServer.Services
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        public string MediaShareServer { get; set; }
        public JwtSettings JwtSettings { get; set; }
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
}
