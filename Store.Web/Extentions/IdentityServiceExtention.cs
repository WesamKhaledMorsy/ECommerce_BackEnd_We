using Store.Data.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Store.Data.Contexts;
namespace Store.Web.Extentions
{
    public static class IdentityServiceExtention
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            // specify what we will work on and there is another builder
            var builder = services.AddIdentityCore<AppUser>();
            builder = new IdentityBuilder(builder.UserType,builder.Services);
            // Manage user Persiste (Authentication , authorization ,etc ...)
            builder.AddEntityFrameworkStores<StoreIdentiityDbContext>();
            builder.AddSignInManager<SignInManager<AppUser>>();
            services.AddAuthentication();
            return services;
        }
    }
}
