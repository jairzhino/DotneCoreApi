﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Backend.Model;
using Backend.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.Filters;
using Backend.Utils;

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
                                    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                                    c.AddSecurityDefinition("oauth2", new ApiKeyScheme
                                    {
                                        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                                        In = "header",
                                        Name = "Authorization",
                                        Type = "apiKey"
                                    });
                                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                                });
            
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            ServiceJwt.ConfigureJwt(services,Configuration);

            services.AddDbContext<ContextDB>(opt => opt.UseSqlite("Filename=database.db"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.ConfigureExceptionHandler();//Error Handler for all throw exception.
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)//Error Handler for EndPoint exception.
                {
                    throw new Exception("Request Not Found, or path not exist (" + context.Request.Path + ")");
                }
            });
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
