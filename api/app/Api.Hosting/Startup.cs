using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Api.Hosting.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetCoreStack.Proxy;
using Swashbuckle.AspNetCore.Swagger;

namespace Api.Hosting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.AddNetCoreProxy(Configuration, options =>
            {
                // Register the API to use as a Proxy
                options.Register<IConsulApi>();
            });

            services.Configure<DbSettings>(Configuration.GetSection(nameof(DbSettings)));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Hosting API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hosting API v1");
            });

            app.UseMvc();

            try
            {
                using(var scope = app.ApplicationServices.CreateScope()) 
                {
                    var api = scope.ServiceProvider.GetService<IConsulApi>();
                    var address = GetLocalIPAddress();
                    var port = 5000;

                    Task.Run(async () => {
                        await api.RegisterAsync(new AgentRegistry 
                        {
                            ID = Environment.MachineName,
                            Name = "Web" + Environment.MachineName,
                            Port = port,
                            Tags = new List<string> { "web", "app", "v1" },
                            Address = address,
                            EnableTagOverride = false,
                            Meta = new Dictionary<string, string> 
                            {
                                ["dotnet_version"] = "2.1"
                            },
                            Check = new Check {
                                HTTP = $"http://{address}:{port}/api/health",
                                Interval = "10s"
                            }
                        });
                    }).GetAwaiter().GetResult();
                }
            } 
            catch 
            {

            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            
            return "127.0.0.1";
        }
    }
}
