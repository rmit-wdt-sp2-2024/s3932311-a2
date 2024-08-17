using Microsoft.EntityFrameworkCore;
using MvcMCBA.Data;
using MvcMCBA.Models;
using MvcMCBA.Filters;

namespace MvcMCBA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MvcMCBAContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMCBAContext"));
                options.UseLazyLoadingProxies();
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AuthorizeCustomerAttribute>();
            });

            var app = builder.Build();

            // Seed data.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
