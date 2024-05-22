﻿using Microsoft.AspNetCore.Builder;

namespace NorthwindApp.Middleware
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
            /* .UseMiddleware<HelperMiddleware>()*/


        }
    }
}
