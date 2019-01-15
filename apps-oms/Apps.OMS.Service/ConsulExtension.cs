using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Apps.OMS.Service
{
    public static class ConsulExtension
    {
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://localhost:8500"));//请求注册的 Consul 地址
            var httpCheck = new AgentServiceCheck()
            {

                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册

                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔

                HTTP = $"http://192.168.1.176:1996/api/health",//健康检查地址

                Timeout = TimeSpan.FromSeconds(5)
            };

            var registration = new AgentServiceRegistration()
            {

                Checks = new[] { httpCheck },

                ID = "omsservice1",

                Name = "dmzomsservice",

                Address = "192.168.1.176",

                Port = 1996,

                Tags = new[] { $"urlprefix-/mymy" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）

            lifetime.ApplicationStopping.Register(() =>
            {

                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册

            });

            return app;

        }

    }
}
