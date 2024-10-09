using Store.Data.Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Store.Data.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace Store.Web.Extentions
{
    public static class IdentityServiceExtention
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration _config)
        {
            // specify what we will work on and there is another builder
            var builder = services.AddIdentityCore<AppUser>();
            
            builder = new IdentityBuilder(builder.UserType,builder.Services);
            // Manage user Persiste (Authentication , authorization ,etc ...)
            builder.AddEntityFrameworkStores<StoreIdentiityDbContext>();
            
            builder.AddSignInManager<SignInManager<AppUser>>();
            /// Should Add this configurstions here to Valid the authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters =new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, // make Validations on SigningKey  ["Token:Key]
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"])),
                        ValidateIssuer = true,
                        ValidIssuer=_config["Token:Issuer"],
                        ValidateAudience = false,
                    };
                });
            
            return services;
        }
    }
}
