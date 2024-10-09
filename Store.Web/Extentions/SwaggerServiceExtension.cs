using Microsoft.OpenApi.Models;
namespace Store.Web.Extentions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                       Title="Store App",
                       Version="v1",
                       Contact= new OpenApiContact 
                       {
                            Name="Wesam Morsy",
                            Email="wesamkhaledmorsy@gmail.com"
                       }
                    });
                // Create Popup to add token
                var securityScheme = new OpenApiSecurityScheme
                {
                    Description="JWT Autherization header using the Bearer scheme. Example:\"Autherization: Bearer {token}\"",
                    Name = "Authorization", // Header
                    In= ParameterLocation.Header, // pass the token in the header of the request
                    Type= SecuritySchemeType.ApiKey,// From type Api key
                    Scheme = "bearer", // Type of Authorization of JWT ,and type word bearer before it
                    Reference = new OpenApiReference
                    {
                        Id="bearer",
                        Type=ReferenceType.SecurityScheme 
                    }
                };
                options.AddSecurityDefinition("bearer",securityScheme);
                var securityRequirements = new OpenApiSecurityRequirement
                {
                    {securityScheme, new []{"bearer"} } // to be like this >>> Bearer {token}
                };
                options.AddSecurityRequirement(securityRequirements);
            });

            return services;
        }
    }
}
