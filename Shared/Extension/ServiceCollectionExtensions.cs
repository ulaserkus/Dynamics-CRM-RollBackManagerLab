using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Api.Abstract;
using Shared.Api.Concrete;
using Shared.Authentication.Basic;
using Shared.Config.Abstract;
using Shared.Config.Concrete;
using Shared.Contants;
using Shared.Repository.Abstract;
using Shared.Repository.Concrete;

namespace Shared.Extension
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

            return services;
        }

        public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtConfig>(configuration.GetSection(nameof(JwtConfig)));
            services.AddSingleton<IJwtConfig>(sp =>
            {
                return sp.GetRequiredService<IOptions<JwtConfig>>().Value;
            });

            return services;
        }

        public static IServiceCollection AddSmtpConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpConfig>(configuration.GetSection(nameof(SmtpConfig)));
            services.AddSingleton<ISmtpConfig>(sp =>
            {
                return sp.GetRequiredService<IOptions<SmtpConfig>>().Value;
            });

            return services;
        }

        public static IServiceCollection AddBasicAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBasicApiUserRepository, BasicApiUserRepository>();

            services.AddAuthentication("BasicAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            return services;
        }

        public static IServiceCollection RegisterApiAndClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient(AppSecureConstants.IdentityApiClientName, client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("AppAddresses:Identity.WebApi").Value);
            });
            services.AddHttpClient(AppSecureConstants.QueueApiClientName, client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection("AppAddresses:Queue.WebApi").Value);
            });
            services.AddScoped<IIdentityApiHandler, IdentityApiHandler>();
            services.AddScoped<IQueueApiHandler, QueueApiHandler>();
            return services;
        }
    }
}
