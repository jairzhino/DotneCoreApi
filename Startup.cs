using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Backend.Model;
using Backend.Middleware;
using Swashbuckle.AspNetCore.Filters;
using Backend.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Backend
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
            
            services.AddSwaggerGen(c =>
                                {
                                    c.SwaggerDoc("v1", new  OpenApiInfo { Title = "My API", Version = "v1" });
                                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                                    {
                                        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                                        In = ParameterLocation.Header,
                                        Name = "Authorization",
                                        Type = SecuritySchemeType.ApiKey
                                    });
                                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                                });
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            ServiceJwt.ConfigureJwt(services,Configuration);

            services.AddDbContext<ContextDB>(opt => opt.UseSqlite(Configuration.GetConnectionString("sqlite")));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.ConfigureExceptionHandler();//Error Handler for all throw exception.
            
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
