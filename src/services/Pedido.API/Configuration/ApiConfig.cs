namespace Pedido.API.Configuration
{
    public static class ApiConfig
    {
        public static void AddApiConfig(this IServiceCollection services)
        {
            services.AddControllers();
        }
    
        public static void UseApiConfig(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
