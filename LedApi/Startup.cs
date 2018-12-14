using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using LedApi.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Camera;

namespace LedApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();
            services.Configure<LedApiConfiguration>(Configuration.GetSection("LedApiConfiguration"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            //app.UseForwardedHeaders(new ForwardedHeadersOptions
            //{
            //    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            //});
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets(webSocketOptions);
            app.Use(async (http, next) =>
            {
                if (http.Request.Path == "/ws")
                {
                    if (http.WebSockets.IsWebSocketRequest)
                    {
                        var token = CancellationToken.None;
                        var buffer = new byte[4096];
                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                        var client = new TcpClient("192.168.1.152", 8080);
                        var stream = client.GetStream();
                        while (webSocket.State == WebSocketState.Open)
                        {
                            await stream.ReadAsync(buffer);
                            await webSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, token);
                        }
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            });
            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
