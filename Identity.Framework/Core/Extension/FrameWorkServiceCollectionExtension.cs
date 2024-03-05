using Identity.Framework.Core.Mapper.Abstract;
using Identity.Framework.Core.Mapper.Concrete;
using Identity.Framework.Core.Services.Abstract;
using Identity.Framework.Core.Services.Concrete;
using Identity.Framework.Core.Services.JWT;
using Identity.Framework.Data.Repository.Abstract;
using Identity.Framework.Data.Repository.Concrete;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contants;
using Shared.Model.Concrete;
using Shared.Repository.Abstract;
using Shared.Repository.Concrete;
using Shared.Utils;

namespace Identity.Framework.Core.Extension
{
    public static class FrameWorkServiceCollectionExtension
    {
        public static IServiceCollection AddFrameworkServices(this IServiceCollection services)
        {
            //Inject Services Here
            services.AddScoped<IBasicApiUserRepository, BasicApiUserRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            //****
            return services;
        }
        public static void SaveOrUpdateIdentityApiUser(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var basicUserRepo = serviceProvider.GetRequiredService<IBasicApiUserRepository>();
                var apiUser = new BasicApiUser { ApiName = AppSecureConstants.IdentityApiClientName, HashedPassword = HashUtil.GenerateHash(EncryptUtil.Decrypt(AppSecureConstants.IdentityApiPasswordCrypted), out string salt), Salt = salt, UserName = AppSecureConstants.IdentityApiUserName };

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
            services.AddScoped<IUserMapper, UserMapper>();
            return services;
        }
    }
}
