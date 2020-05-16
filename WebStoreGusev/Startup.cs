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
        #region Подключение конструктора принимающего IConfiguration

        /// <summary>
        /// Свойство для доступа к конфигурации.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Конструктор, принимающий интерфейс IConfiguration.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        public void ConfigureServices(IServiceCollection services)
        {
            // Подключение MVC
            services.AddMvc();

            #region Глобальные фильтры

            // Подключение фильтра ко всем контроллерам и всем Action-методам.

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(SampleActionFilter));

            // Aльтернативный вариант подключения

            //options.Filters.Add(new SampleActionFilter());
            //});

            #endregion

            #region Подключение БД

            services.AddDbContext<WebStoreContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #endregion

            #region Подключение Identity

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            // необязательно
            services.Configure<IdentityOptions>(options =>
            {
                // Настройки пароля
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                // Настройки локаута
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // Настройки пользователя
                options.User.RequireUniqueEmail = true;
            });

            // необязательно
            // Настройки куков
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

            #region Подключаем разрешение зависимости

            // Добавляем разрешение зависимости

            // Методы указывают на время жизни сервиса

            // Singleton - будет жить все время жизни проекта
            services.AddSingleton<IEmployeesService, InMemoryEmployeeService>();
            services.AddSingleton<ICarsService, InMemoryCarService>();
            //services.AddSingleton<IProductService, InMemoryProductService>();
            
            services.AddScoped<IProductService, SqlProductService>();
            services.AddScoped<IOrdersService, SqlOrdersService>();

            // Настройки для корзины
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ICartService, CookieCartService>();

            // Scoped - время жизни Http запроса
            //services.AddScoped<IEmployeesService, InMemoryEmployeeService>();

            // Transient - пересоздает сервис при каждом запросе
            //services.AddTransient<IEmployeesService, InMemoryEmployeeService>();

            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Режим разработчика
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Подключение статических ресурсов.
            app.UseStaticFiles();

            app.UseRouting();

            // Подключение аутентификации
            app.UseAuthentication();

            // Подключение авторизации
            app.UseAuthorization();

            // устанавливать кастомные обработчики
            app.Map("/index", CustomIndexHandler);

            app.UseMiddleware<TokenMiddleware>();

            // Можно прописать логику 
            // "останавливать выполнение запроса или продолжать".
            UseSample(app);

            #region Настройка маршрутизации MVC

            // Настройка маршрутизации MVC
            app.UseEndpoints(endpoints =>
            {
                // Подключение Area
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                // Подключение MVC 3.1
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Аналогичное подключение MVC
            //app.UseMvcWithDefaultRoute();

            #endregion

            #region Подключение приветственной страницы

            // Подключение приветственной страницы.
            app.UseWelcomePage();
            // Приветственная страница доступна только по адресу /welcome
            //app.UseWelcomePage("/welcome");

            #endregion

            // Run заканчивает обработку запроса
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
                await context.Response.WriteAsync("Привет из конвейера обработки запроса (метод app.Run())");
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
