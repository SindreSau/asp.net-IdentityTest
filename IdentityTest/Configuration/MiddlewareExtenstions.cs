using IdentityTest.Data;
using IdentityTest.Models;

namespace IdentityTest.Configuration
{
    public static class MiddlewareExtensions
    {
        public static void SwaggerUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();
        }
    }
}