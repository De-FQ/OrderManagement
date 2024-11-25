using jsreport.AspNetCore;
using jsreport.Local;
using jsreport.Types;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Serilog;
using System.Globalization;
using System.Runtime.InteropServices;
using Utility.API;
using Utility.Helpers;

namespace Web.Extensions
{
    /// <summary>
    /// Dependency Register
    /// </summary>
    public static class CommonServiceExtensions
    {
       

        /// <summary>
        /// Customize - Add Authentication Cookie Service
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthenticationCookieService(this IServiceCollection services)
        {
            var ExpiryMinutes = 43800; 
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = Constants.Cookie.AuthenticationScheme;
                options.DefaultAuthenticateScheme = Constants.Cookie.AuthenticationScheme;
                options.DefaultChallengeScheme= Constants.Cookie.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.ExpireTimeSpan =  TimeSpan.FromMinutes(ExpiryMinutes);
                options.Cookie.MaxAge = options.ExpireTimeSpan;
                options.SlidingExpiration = true;
            });
        }
        /// <summary>
        /// Customize - for custom policy, add policy names inside method
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Root", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("Department", "HR");
                });

                options.AddPolicy("Administrator", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("Department", "IT");
                });
            });

        }

        #region Localization

        /// <summary>
        /// Customize - Add multi language support (english & arabic)
        /// </summary>
        /// <param name="services"></param>
        public static void AddLocalizationService(this IServiceCollection services)
        {
            

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<IStringLocalizer, StringLocalizer<SharedResource>>();
            services.AddMvc()
                                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                                .AddDataAnnotationsLocalization(o =>
                                {
                                    o.DataAnnotationLocalizerProvider = (type, factory) =>
                                    {
                                        return factory.Create(typeof(SharedResource));
                                    };
                                });

            var cultures = new List<CultureInfo> {
                    new CultureInfo("ar"),
                    new CultureInfo("en")
                };


            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                };
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        #endregion Localatization
        /// <summary>
        /// Customize - Dependency Injection - Add default application scope
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="Exception"></exception>
        public static void AddDependencyInjectionService(this IServiceCollection services, IConfiguration configuration)
        {
         
            // need more info
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });


            #region Dependency Injection (DI) 
            AppSettingsModel appSettings = configuration.Get<AppSettingsModel>();

            if (appSettings == null) throw new Exception("Add Dependency Injection AppSettingsModel fail"); //fail early

            services.TryAddSingleton(appSettings);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAPIHelper, APIHelper>();
            services.AddScoped<IEncryptionServices, EncryptionServices>();
             
            #endregion Dependency Injection
        }
        #region Add Session Service
        /// <summary>
        /// If you add "AddSessionService" than you must add "UseSessionService" statement
        /// Customize - Enable caching for session state (30 minutes)
        /// </summary>
        public static void AddSessionService(this IServiceCollection services )
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }); 
        }
        /// <summary>
        /// If you add "AddSessionService" than you have use it  "UseSessionService" 
        /// </summary>
        /// <param name="app"></param>
        public static void UseSessionService(this IApplicationBuilder app)
        { 
            app.UseSession(); 
        }
        #endregion Add Session Service


        /// <summary>
        /// custom - 
        /// <para>Note: ILoggerManager should be added in Dependency Injection
        /// Capture all error logs in "logs/applog.txt" throughout the api
        /// The Log class object is available in api and its used inside ConfigureExceptionHandler method</para>
        /// </summary>
        /// <param name="app"></param>
        public static void AddSeriLogger(this WebApplicationBuilder builder)
        {
            #region All Exceptions configuration

            Log.Logger = new Serilog.LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

            //    .MinimumLevel.Information()
            //    .WriteTo.Console()
            //    .WriteTo.File("logs/applog.txt", rollingInterval: RollingInterval.Day)
            //    .CreateLogger();
            //var logger = app.Services.GetRequiredService<ILoggerManager>();
            //app.ConfigureExceptionHandler(logger);

            #endregion
        }
         
    }
}
