using _95PhrEAKer.Services.IServices.EmailServices;
using _95PhrEAKer.Services.IServices.RelationalService;
using _95PhrEAKer.Services.IServices.UserPostService;
using _95PhrEAKer.Services.IServices.UserServices;
using _95PhrEAKer.Services.ServicesExtension.EmailServices;
using _95PhrEAKer.Services.ServicesExtension.RelationalServices;
using _95PhrEAKer.Services.ServicesExtension.UserPostService;
using _95PhrEAKer.Services.ServicesExtension.UserServices;

namespace _95PhrEAKer_webapi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserPostService, UserPostsService>();
            services.AddScoped<IUser, UserService>();
            services.AddScoped<IRelationalService, RelationalService>();

            return services;
        }
    }
}
