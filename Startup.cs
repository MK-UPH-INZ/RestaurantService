using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RestaurantService.AsyncDataServices;
using RestaurantService.Data;
using RestaurantService.EventProcessing;
using RestaurantService.Extension;
using RestaurantService.SyncDataServices.Grpc;

namespace RestaurantService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment env;

        public Startup(
            IConfiguration configuration, 
            IWebHostEnvironment env
        )
        {
            Configuration = configuration;
            this.env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (env.IsProduction())
            {
                Console.WriteLine("--> Using MSSQL DB");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("RestaurantServiceConn"))
                );
            }
            else
            {
                Console.WriteLine("--> Using InMem DB");
                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMem")
                );
            }

            services.AddScoped<IRestaurantRepo, RestaurantRepo>();
            services.AddScoped<IUserRepo, UserRepo>();

            services.AddCustomJwtAuthentication(Configuration["JwtKey"]);

            services.AddSingleton<IMessageBusClient, MessageBusClient>();
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddScoped<IUserDataClient, UserDataClient>();
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHostedService<MessageBusSubscriber>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
