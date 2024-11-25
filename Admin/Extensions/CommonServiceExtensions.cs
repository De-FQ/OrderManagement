using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Security.Policy;
using Utility.API;
using Utility.Helpers;

namespace Admin.Extensions
{
    /// <summary>
    /// Dependency Register
    /// </summary>
    public static class CommonServiceExtensions
    {
        /// <summary>
        /// This method shall be added in API for Admin-API project
        ///<para> If the service is configured in the following way, <br></br>
        ///services.AddAntiforgery();
        ///<br></br> it means that the default values will be used: when this kind of scenario occurs, 
        ///the hidden <b>  input type="hidden" </b> that gets automatically appended to each 
        ///<br></br>Razor Page or View <b>form</b> has a name equals to __RequestVerificationToken.</para>
        /// <see cref="https://www.ryadel.com/en/asp-net-core-antiforgery-token-jquery-ajax-post-405/"/> 
        /// </summary>
        /// <param name="services"></param>
        public static void AddAntiForgeryService(this  IServiceCollection services)
        { 
            services.AddAntiforgery(options => {
                //options.Cookie.Name = "X-CSRF-TOKEN";
                options.HeaderName = "X-CSRF-TOKEN";
                //options.FormFieldName = "X-CSRF-TOKEN";
            });
            services.AddControllersWithViews(options =>
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
        }
         
        /// <summary>
        /// Customize - Add Authentication Cookie Service and Add Authorization
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthenticationCookieService(this IServiceCollection services)
        {
            //var OneDayInMinutes = 1 * 60 * 1000;//87600
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = Constants.Cookie.AuthenticationScheme;
                options.DefaultAuthenticateScheme = Constants.Cookie.AuthenticationScheme;
                options.DefaultChallengeScheme = Constants.Cookie.AuthenticationScheme;
            }) .AddCookie(options =>
             {
                 //    options.LoginPath = "/account/SessionExpired";
                 options.ExpireTimeSpan = TimeSpan.FromDays(1);
                 //options.Cookie.MaxAge = options.ExpireTimeSpan;
                 options.SlidingExpiration = true;
                options.EventsType = typeof(CustomCookieAuthenticationEvents);
             });
             services.AddScoped<CustomCookieAuthenticationEvents>();

            services.AddAuthorization();
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
        /// If you add "AddSessionService" than you have use it  "UseSessionService" 
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
        /// <br>
        /// <b>Configure JSON serializer options</b>
        /// </br> 
        /// <br>ASP.NET Core uses System.Text.Json as the default JSON serializer. To configure the JSON serializer options, call AddJsonOptions() in the initialization code 
        /// </br>
        /// <br>
        /// <b>for reference: <see cref="https://makolyte.com/aspdotnet-how-to-change-the-json-serialization-settings/"/> </b>
        /// </br>
        /// <br>
        /// Package installed:  PackageReference Include='Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="7.0.4" </br>
        /// <br>
        /// Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles
        /// </br>
        /// </summary>
        /// <param name="app"></param>
        public static void AddControllersWithAddNewtonsoftJson(this IServiceCollection services)
        {
            services.AddControllers()
               .AddControllersAsServices()
               .AddNewtonsoftJson(options =>
               {
                   // To prevent "A possible object cycle was detected which is not supported" error
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                   // To get our property names serialized in the first letter lowercased
                   //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
               });

        }
    }

    
    }
