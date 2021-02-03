using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservasDeCine.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ReservasDeCine
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

         // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = @"Server=LOCALHOST\SQLEXPRESS;Database=ReservasDeCine;User ID=ReservasDeCineAdmin;Password=alan2812;Trusted_Connection=True;";
            services.AddDbContext<ReservasDeCineDbContext>(options => options.UseSqlServer(connectionString));
            //services.AddDbContext<ReservasDeCineDbContext>(options => options.UseSqlite("filename=ReservasDeCine.db"));

            // AR esto lo necesito para la autenticación de lo contrario me da el error
            // Excepción interna 1:
            // InvalidOperationException: No sign-in authentication handlers are registered. Did you forget to call AddAuthentication().AddCookies("Cookies", ...) ?
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Accesos/Login";
                options.AccessDeniedPath = "/Accesos/NoAutorizado";
                options.LogoutPath = "/Accesos/Logout";
                options.ExpireTimeSpan = new System.TimeSpan(2, 0, 0);
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
        }
    }
}
