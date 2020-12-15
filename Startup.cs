using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace ApiGateway
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
         
            services.AddOcelot().AddConsul();//.AddPolly();
            services.AddAuthentication()
                .AddIdentityServerAuthentication("UserServiceGateway",
                    options =>
                    {
                        options.Authority = "http://"+ Configuration["IdentityServer4:IP"] + ":" + Configuration["IdentityServer4:Port"];
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "UserServiceApi";
                        options.ApiSecret = "secret";
                        options.SupportedTokens = SupportedTokens.Both;
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            app.UseOcelot();//.Wait();//ocelot请求管道

            ServiceEntity serviceEntity = new ServiceEntity
            {
                IP = Configuration["Service:IP"],
                Port = Convert.ToInt32(Configuration["Service:Port"]),
                ServiceName = Configuration["Service:Name"],
                ConsulIP = Configuration["Consul:IP"],
                ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])

            };
            Console.WriteLine($"consul开始注册{JsonConvert.SerializeObject(serviceEntity)}");
            app.RegisterConsul(lifetime, serviceEntity);

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //app.UseRouting();
            //微服务中的各个swagger名称
            //var apis = new List<string> { "UserService" };
            //app.UseMvc()
            //    .UseSwagger()
            //    .UseSwaggerUI(options=>
            //    { 
            //    apis.ForEach(m=>
            //    options.SwaggerEndpoint()

            //    )
            //    });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
            //MvcOptions m = new MvcOptions();
            //m.EnableEndpointRouting = false;
        }
    }
}
