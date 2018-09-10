namespace ApiServer.Services
{
    /// <summary>
    /// 应用配置
    /// </summary>
    public class AppConfig
    {
        public string MediaShareServer { get; set; }
        public string MessageMail { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public SMTPSettings SMTPSettings { get; set; }
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

    public class SMTPSettings
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string Hosts { get; set; }
        public int Port { get; set; }
    }
}
