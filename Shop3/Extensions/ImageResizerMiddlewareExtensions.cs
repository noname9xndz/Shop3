using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shop3.Middleware;

namespace Shop3.Extensions
{
    // https://www.paddo.org/asp-net-core-image-resizing-middleware/ 
    // exten using ImageResizerMiddleware để sử dụng
    // https://docs.microsoft.com/vi-vn/aspnet/core/fundamentals/middleware/write?view=aspnetcore-2.2
    public static class ImageResizerMiddlewareExtensions
    {
        public static IServiceCollection AddImageResizer(this IServiceCollection services)
        {
            return services.AddMemoryCache();
        }

        public static IApplicationBuilder UseImageResizer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageResizerMiddleware>();
        }
    }
}
