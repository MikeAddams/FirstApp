using Managers;
using Repositories;
using Services;
using Managers.Interfaces;
using Services.Interfaces;
using Repositories.Interfaces;
using Repositories.DataContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using App.Infrastructure.Interfaces;
using App.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

namespace App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RepoDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LocalConnection"));
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IManagerService, ManagerService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IProductManager, ProductManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "UserSessionCookie";
                    options.LoginPath = "/Account/Login";
                    //options.LogoutPath = "/Account/Profile";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(25);
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
                /*
                app.UseExceptionHandler(app =>
                {
                    app.Run(async context =>
                    {
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is PermissionExceptions)
                        {
                            app.UseExceptionHandler("/error/nopermission");
                        }
                        else
                        {
                            app.UseExceptionHandler("/error/crash");
                        }
                    });
                });
                */

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "notFound",
                    pattern: "NotFoundPage",
                    defaults: new { controller = "Home", action = "NotFoundPage" }
                );

                endpoints.MapControllerRoute(
                    name: "category",
                    pattern: "{category}/{subCategory?}",
                    defaults: new { controller = "Home", action = "CategoryProducts" });
            });
        }
    }
}
