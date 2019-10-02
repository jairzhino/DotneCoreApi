using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Backend.Utils{
    public static class ServiceJwt
    {
        public static void ConfigureJwt(IServiceCollection services, IConfiguration Configuration){
            var appSettings = Configuration.GetSection("AppSettings:keyJwt");
            
            

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
                            GetBytes(appSettings.Value))
                    };
                    options.Events = new JwtBearerEvents //Error Handling For Authentication Layer.
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            
                            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
                            dic.Add("Forbbiden", new string[] { 
                                "Error the Token is invalid or forbbiden for this Controller(" + context.Request.Path + ")",
                                context.Error });
                            
                            //var response = new Response(HttpStatusCode.Forbidden, "Your session has ended due to inactivity");
                            context.Response.ContentType = "application/json";
                            ValidationProblemDetails vp = new ValidationProblemDetails(dic);
                            vp.Status = (int)HttpStatusCode.Forbidden;
                            vp.Title = "Error Auth";
                            //return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(vp));
                        }
                        
                    };
                    
                }
                
            );
        }
    }
}