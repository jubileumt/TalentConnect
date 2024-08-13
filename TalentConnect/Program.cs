using Microsoft.EntityFrameworkCore;
using TalentConnect.Data;

namespace TalentConnect
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure the database connection
            string connectionString = "server=localhost;port=3306;database=TalentConnect;user=root;";
            builder.Services.AddDbContext<TalentConnectBD>(options =>
            {
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 2, 4)));
            });

            // Configure session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Make session cookie HTTP only
                options.Cookie.IsEssential = true; // Essential for cookie consent
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable session middleware
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}