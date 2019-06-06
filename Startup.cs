using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Backend.Model;
using Backend.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "localhost",
                        ValidAudience = "localhost",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                            GetBytes("lkdFJAfakjdklfJFLKASJD34598234789WEFJAEUR83945Q0987RSJDFLKSDJFajlksjdfljierot39847589234jerijakdjfad"))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();

                            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
                            dic.Add("Forbbiden", new string[] { "Error the Token is invalid or forbbiden for this Controller("+context.Request.Path+")" });
                            //var response = new Response(HttpStatusCode.Forbidden, "Your session has ended due to inactivity");
                            context.Response.ContentType = "application/json";
                            ValidationProblemDetails vp = new ValidationProblemDetails(dic);
                            vp.Status = (int)HttpStatusCode.Forbidden;
                            vp.Title = "Error";
                            //return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(vp));
                        }
                    };
                }
            );

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


            app.ConfigureExceptionHandler();
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
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
