namespace Apps.Base.Common
{
    public class AppConfig
    {
        public string APIGatewayServer { get; set; }
        public string ConnectionString { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public GuidSettings GuidSettings { get; set; }
        public ConsulConfig ConsulConfig { get; set; }
        public Plugins Plugins { get; set; }
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

    /// <summary>
    /// GUID配置
    /// </summary>
    public class GuidSettings
    {
        public int ServerId { get; set; }
        public string GuidSalt { get; set; }
        public int GuidMinLen { get; set; }
    }

    public class Plugins
    {
        public string OrderViewer { get; set; }
        public string MediaShare { get; set; }
    }

    public class ConsulConfig
    {
        public NodeMember Server { get; set; }
        public NodeMember Client { get; set; }
    }

    public class NodeMember
    {
        public string IP { get; set; }
        public string Port { get; set; }
    }

}
