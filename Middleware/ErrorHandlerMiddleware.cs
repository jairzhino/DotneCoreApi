
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Backend.Middleware
{
    public static class ErrorHandlerMiddleware
    {

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(
                appError =>
                {
                    appError.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            if (!(context.Response.StatusCode >= 400 && context.Response.StatusCode < 600))
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";
                            IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                            if (contextFeature != null)
                            {
                                Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
                                dic.Add("exception", new string[] { contextFeature.Error.Message });

                                ValidationProblemDetails vp = new ValidationProblemDetails(dic);
                                vp.Status=context.Response.StatusCode;
                                vp.Title = "Error";
                                await context.Response.WriteAsync(JsonConvert.SerializeObject(vp));
                            }
                        }
                    );
                }
            );

        }
    }
}