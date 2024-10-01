
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.Data.Contexts;
using Store.Web.Extentions;
using Store.Web.Helper;
using Store.Web.Middlewares;

namespace Store.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            // configure caching Redis
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var redisConnection = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(redisConnection);
            });

            // Add the Refactoring method of registering the services
            builder.Services.AddApplicationServices();

            var app = builder.Build();
            // ApplySeeding Method From Helper Folder
            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Add My Custom Middleware before any middleware of routing auth 
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            // To display any(files, images , videos .. ) this in wwwroot
            app.UseStaticFiles();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
