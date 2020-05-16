using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebStoreGusev.DAL;
using WebStoreGusev.DomainNew.Entities;
using WebStoreGusev.Infrastructure;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Infrastructure.Services;

namespace WebStoreGusev
{
    public class Startup
    {
        #region ����������� ������������ ������������ IConfiguration

        /// <summary>
        /// �������� ��� ������� � ������������.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// �����������, ����������� ��������� IConfiguration.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            // ����������� MVC
            services.AddMvc();

            #region ���������� �������

            // ����������� ������� �� ���� ������������ � ���� Action-�������.

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(SampleActionFilter));

            // A������������� ������� �����������

            //options.Filters.Add(new SampleActionFilter());
            //});

            #endregion

            #region ����������� ��

            services.AddDbContext<WebStoreContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #endregion

            #region ����������� Identity

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            // �������������
            services.Configure<IdentityOptions>(options =>
            {
                // ��������� ������
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // ��������� �������
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // ��������� ������������
                options.User.RequireUniqueEmail = true;
            });

            // �������������
            // ��������� �����
            services.ConfigureApplicationCookie(options =>
            {
                //options.Cookie.HttpOnly = true;
                //options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            #endregion

            #region ���������� ���������� �����������

            // ��������� ���������� �����������

            // ������ ��������� �� ����� ����� �������

            // Singleton - ����� ���� ��� ����� ����� �������
            services.AddSingleton<IEmployeesService, InMemoryEmployeeService>();
            services.AddSingleton<ICarsService, InMemoryCarService>();
            //services.AddSingleton<IProductService, InMemoryProductService>();
            
            services.AddScoped<IProductService, SqlProductService>();
            services.AddScoped<IOrdersService, SqlOrdersService>();

            // ��������� ��� �������
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CookieCartService>();

            // Scoped - ����� ����� Http �������
            //services.AddScoped<IEmployeesService, InMemoryEmployeeService>();

            // Transient - ����������� ������ ��� ������ �������
            //services.AddTransient<IEmployeesService, InMemoryEmployeeService>();

            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ����� ������������
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ����������� ����������� ��������.
            app.UseStaticFiles();

            app.UseRouting();

            // ����������� ��������������
            app.UseAuthentication();

            // ����������� �����������
            app.UseAuthorization();

            // ������������� ��������� �����������
            app.Map("/index", CustomIndexHandler);

            app.UseMiddleware<TokenMiddleware>();

            // ����� ��������� ������ 
            // "������������� ���������� ������� ��� ����������".
            UseSample(app);

            #region ��������� ������������� MVC

            // ��������� ������������� MVC
            app.UseEndpoints(endpoints =>
            {
                // ����������� Area
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // ����������� MVC 3.1
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // ����������� ����������� MVC
            //app.UseMvcWithDefaultRoute();

            #endregion

            #region ����������� �������������� ��������

            // ����������� �������������� ��������.
            app.UseWelcomePage();
            // �������������� �������� �������� ������ �� ������ /welcome
            //app.UseWelcomePage("/welcome");

            #endregion

            // Run ����������� ��������� �������
            RunSample(app);
        }

        private void UseSample(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                bool isError = false;
                //...
                if (isError)
                {
                    await context.Response
                        .WriteAsync("Error occured. You're in custom pipline module...");
                }
                else
                {
                    await next.Invoke();
                }
            });
        }

        private void RunSample(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("������ �� ��������� ��������� ������� (����� app.Run())");
            });
        }

        private void CustomIndexHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Index");
            });
        }
    }
}
