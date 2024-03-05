using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Queue.Framework.Core.Mapper.Abstract;
using Queue.Framework.Core.Mapper.Concrete;
using Queue.Framework.Core.Services.Abstract;
using Queue.Framework.Core.Services.Concrete;
using Queue.Framework.Data.Repository.Abstract;
using Queue.Framework.Data.Repository.Concrete;
using Shared.Api.DTOs;
using Shared.Contants;
using Shared.Model.Concrete;
using Shared.Repository.Abstract;
using Shared.Utils;

namespace Queue.Framework.Core.Extension
{
    public static class FrameWorkServiceCollectionExtension
    {
        public static IServiceCollection AddFrameworkServices(this IServiceCollection services)
        {
            //Inject Services Here
            services.AddScoped<IEntityHistoryHashedDataRepository, EntityHistoryHashedDataRepository>();
            services.AddScoped<IEntityHistoryHashedDataService, EntityHistoryHashedDataService>();
            services.AddScoped<IEntityHistoryDtoPub, EntityHistyoryDtoPub>();
            services.AddSingleton<EntityHistoryDtoSub>();
            //****

            return services;
        }

        public static void SaveOrUpdateQueueApiUser(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var basicUserRepo = serviceProvider.GetRequiredService<IBasicApiUserRepository>();
                var apiUser = new BasicApiUser { ApiName = AppSecureConstants.QueueApiClientName, HashedPassword = HashUtil.GenerateHash(EncryptUtil.Decrypt(AppSecureConstants.QueuePasswordCrypted), out string salt), Salt = salt, UserName = AppSecureConstants.QueueApiUserName };

                var findRecord = basicUserRepo.FindOne(x => x.ApiName == apiUser.ApiName);

                if (findRecord != null)
                {
                    basicUserRepo.ReplaceOne(apiUser);
                }
                else
                {
                    basicUserRepo.InsertOne(apiUser);
                }
            }
        }

        public static IServiceCollection AddCustomMappers(this IServiceCollection services)
        {
            services.AddScoped<IEntityHistoryApiDtoMapper, EntityHistoryApiDtoMapper>();
            return services;
        }


        public static IServiceCollection ConfigureRabbitMQ(this IServiceCollection services, IConfiguration config)
        {
            services.AddMassTransit(x =>
            {
                var rabbitCfg = config.GetRequiredSection("RabbitMQConfig");
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(rabbitCfg.GetSection("Host").Value), h =>
                    {
                        h.Username(rabbitCfg.GetSection("UserName").Value);
                        h.Password(rabbitCfg.GetSection("Password").Value);
                    });

                    cfg.ReceiveEndpoint(nameof(EntityHistoryApiDto), e =>
                    {
                        e.PrefetchCount = 100;
                        e.UseMessageRetry(r => r.Interval(2, 100));
                        e.UseRateLimit(100, TimeSpan.FromSeconds(5));
                        e.Consumer<EntityHistoryDtoSub>(context);
                    });
                });
            });

            return services;
        }
    }
}
