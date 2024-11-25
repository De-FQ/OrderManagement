using API.Helpers;
using AutoMapper;
using Data.EntityFramework;
using jsreport.AspNetCore;
using jsreport.Local;
using jsreport.Types;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.Office.Interop.Excel;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using Utility.API;
using Utility.Helpers;
using Utility.LoggerService;


namespace API.Extensions
{
    public static class CommonServiceExtensions
    {



        private static readonly string _defaultCorsPolicyName = "localhost";
        //public static IServiceCollection AddApiAntiforgery( this IServiceCollection services, Action<AntiforgeryOptions> setupAction)
        //{
        //    var types = Assembly.Load("Microsoft.AspNetCore.Mvc.ViewFeatures").GetTypes();
        //    var autoType = types.First(t => t.Name == "AutoValidateAntiforgeryTokenAuthorizationFilter"); // necessary for the AutoValidateAntiforgeryTokenAttribute
        //    var defaultType = types.First(t => t.Name == "ValidateAntiforgeryTokenAuthorizationFilter"); // necessary for the ValidateAntiforgeryTokenAttribute
        //    services.AddSingleton(autoType);
        //    services.AddSingleton(defaultType);
        //    services.AddAntiforgery(setupAction);
        //    return services;
        //}


        //public static void AntiforgeryService(this IApplicationBuilder app,   IAntiforgery antiforgery)
        //{

        //    app.Use(nextDelegate => context =>
        //    {
        //        string path = context.Request.Path.Value.ToLower();
        //        string[] directUrls = { "/admin", "/api" };
        //        if (path.StartsWith("/swagger") || path.StartsWith("/api") || string.Equals("/", path))
        //        {
        //            AntiforgeryTokenSet tokens = antiforgery.GetAndStoreTokens(context);
        //            context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions
        //            {
        //                HttpOnly = false,
        //                Secure = false,
        //                IsEssential = true,
        //                SameSite = SameSiteMode.Strict
        //            });

        //        }
        //        return nextDelegate(context);
        //    });
        //}
        /// <summary>
        /// Custom- 
        /// It should be called before frontend + backend services
        /// <br>HttpContextAccessor, CommonHelper, IAPIHelper, EmailHelper, IEncryptionServices, AppSettingsModel,ICommonHelper</br> 
        /// <br>SQL Database Configuration</br> 
        /// <br>AddJwtAuthentication</br> 
        /// <br>AddCors</br> 
        /// <br>AddJwtAuthorization</br> 
        /// <br>AddSwaggerService</br> 
        /// <br>AddMapper</br> 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddScopedService(this WebApplicationBuilder builder) //this IServiceCollection services, IConfiguration configuration
        {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //helpers Dependency Injection for common services
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
            builder.Services.AddScoped<ICommonHelper, CommonHelper>();
            builder.Services.AddScoped<IAPIHelper, APIHelper>();
            builder.Services.AddScoped<IEncryptionServices, EncryptionServices>();

        }
        /// <summary>
        ///  Custom - Configure Database 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddDatabaseContext(this WebApplicationBuilder builder)
        {
            var conString = builder.Configuration.GetConnectionString("DefaultConnectionString");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conString, b => b.MigrationsAssembly("Data")));
            //        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
        }

        #region JWT Authentication + Authorization
        /// <summary>
        /// Custom - JWT Authentication + Authorization
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="appSettings"></param>
        public static void AddJwtAuthentication(this WebApplicationBuilder builder, AppSettingsModel appSettings)
        {
            var apiKey = Encoding.UTF8.GetBytes(appSettings.JwtSettings.APIKey);
            var decryptionKey = Encoding.UTF8.GetBytes(appSettings.JwtSettings.SecretKey);
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
           .AddJwtBearer(options =>
           {
               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   IssuerSigningKey = new SymmetricSecurityKey(apiKey),
                   ValidateIssuerSigningKey = true,

                   ValidIssuer = appSettings.JwtSettings.Issuer.ToString(),
                   ValidateIssuer = true,

                   ValidAudience = appSettings.JwtSettings.Audience.ToString(),
                   ValidateAudience = true,

                   TokenDecryptionKey = new SymmetricSecurityKey(decryptionKey),

                   NameClaimType = JwtRegisteredClaimNames.Sub, //BKD new addition for role based auto authenticate
                   RoleClaimType = ClaimTypes.Role,//BKD new addition for role based auto authenticate

                   // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time
                   // when validating the lifetime. As we're creating the tokens locally and validating them on the same
                   // machines which should have synchronised time, this can be set to zero. and default value will be 5 minutes
                   ClockSkew = TimeSpan.Zero,

                   //ValidateLifetime = true,

                   //RequireExpirationTime = true,
                   //
               };

               options.Events = new JwtBearerEvents
               {
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Cookies[Utility.Helpers.Constants.ClaimTypes.AuthenticationToken];
                       //var unencryptedToken = context.Token;
                       var path = context.HttpContext.Request.Path;
                       //for hub request
                       if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments(Utility.Helpers.Constants.Common.APIForUrlHub)))
                       {
                           // Read the token out of the query string
                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   },
                   //OnChallenge = context =>
                   //{
                   //    return Task.CompletedTask;
                   //},

                   OnAuthenticationFailed = context =>
                   {

                       var errorType = context.Exception.GetType();
                       if (errorType == typeof(SecurityTokenExpiredException))
                       {
                           context.Response.Headers.Add("Token-Expired", "true");
                       }
                       else if (errorType == typeof(SecurityTokenException))
                       {
                           context.Response.Headers.Add("TOKEN-EXCEPTION", "true");
                       }
                       else if (errorType == typeof(SecurityTokenDecryptionFailedException))
                       {
                           context.Response.Headers.Add("TOKEN-DECRYPTION-FAILED", "true");
                       }
                       else if (errorType == typeof(SecurityKeyIdentifierClause))
                       {
                           context.Response.Headers.Add("KEY-IDENTIFIER", "true");
                       }
                       else
                       {
                           context.Response.Headers.Add("TOKEN-KEY-ERROR", "true");
                       }
                       return Task.CompletedTask;
                   },

               };
           });
        }
        /// <summary>
        /// Custom - JWT  Authorization
        /// </summary>
        /// <param name="builder"></param>
        public static void AddJwtAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
        }
        #endregion JWT Authentication + Authorization

        #region Cookie Authentication + Authorization 
        #endregion Cookie Authentication + Authorization
        /// <summary>
        /// Custom - Swagger Service
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSwaggerService(this WebApplicationBuilder builder)
        {
            //more configuration
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Management System 1.0", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                  });

                c.OperationFilter<CustomHeaderSwaggerAttribute>();
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }
        /// <summary>
        /// Custom - Add Cors Policy
        /// <b>Note: Do not remove "OPTIONS" because it will use for preflight query</b>
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="appSettings"></param>
        public static void AddCors(this WebApplicationBuilder builder, AppSettingsModel appSettings)
        {
            string CorsAllowedUrls = appSettings.AppSettings.CorsAllowedUrls;

            builder.Services.AddCors(
             options => options.AddPolicy(
                 _defaultCorsPolicyName,
                 builder => builder
                     .WithOrigins(
                         CorsAllowedUrls
                             .Split(",", StringSplitOptions.RemoveEmptyEntries)
                             .ToArray()
                     )
                     .SetIsOriginAllowedToAllowWildcardSubdomains()
                     .AllowAnyHeader()
                     .AllowCredentials()
                     .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS") //do not remove "OPTIONS" because it will use for preflight
                     .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))
             )
         );




        }
        /// <summary>
        /// Custom - Add Profile Auto Mapper
        /// </summary>
        /// <param name="builder"></param>
        public static void AddMapper(this WebApplicationBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
        }

        /// <summary>
        /// Custom - Add Static folders in the API Project
        /// <para>
        /// <b> Access Control Allow Origin is added due to .svg extension images are not showing in Website</b>
        /// <b><br></br>context.Context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
        /// </b><br></br>
        /// <b>Error</b><br></br>
        /// localhost/:1553 Access to image at 'https://localhost:7005/Images/Attractions/Icon/recreational-and-entertainment.svg' from origin 'https://localhost:7251' has been blocked by CORS policy: No 'Access-Control-Allow-Origin' header is present on the requested resource.
        /// </para>
        /// </summary>
        /// <param name="app"></param>
        public static void UseStaticFiles(this WebApplication app)
        {
            //image creation folder 
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Contents")),
                RequestPath = new PathString("/Contents"),
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=31536000";
                    context.Context.Response.Headers[HeaderNames.Expires] = DateTime.UtcNow.AddYears(1).ToString("R");
                    context.Context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = "*";
                }

            });
        }
        /// <summary>
        /// Custom - Use default Cors Policy (localhost)
        /// </summary>
        /// <param name="app"></param>
        public static void UseCors(this WebApplication app) //IApplicationBuilder app)
        {
            app.UseCors(_defaultCorsPolicyName);

        }



        /// <summary> 
        ///<para><b>X-XSS-Protection  </b>
        ///X-XSS-Protection header is for protecting your site from XSS (Cross-site scripting) 
        ///attacks.If a cross - site scripting attack is detected, the browser will sanitize the 
        ///page and the malicious part will either be removed OR the browser will prevent rendering 
        ///of the page and will block an attack(mode=block).
        ///<br><see cref="https://www.thecodebuzz.com/asp-net-core-security-headers-hsts-x-content-type-options-content-security-policy-x-xss-protection/"/>
        ///</br></para> 
        ///<para><br></br><b>Content-Security-Policy</b>
        ///A Content-Security - Policy(CSP) header enables you to control the sources / content on your site that the browser can load. So this header gives you the ability to load the only resources needed by the browser.
        ///A Content Security Policy(CSP) helps protect against XSS attacks by informing the browser of valid re-sources like as below,
        ///Content, scripts, stylesheets, and images.
        ///Actions are taken by a page, specifying permitted URL targets of forms.
        ///Plugins that can be loaded.
        ///<list type="">
        ///<item>default-src 'self'; <para>Fallback for other fetch directives.</para></item>
        ///<item>img-src data: https: http:;<para>Allows the components to load specific images and document pages.</para></item>
        ///<item>script-src 'self' 'unsafe-inline';<para>Allows only scripts loaded from the same source as the current page protected with CSP. You can implement a nonce-based CSP to remove the unsafe-inline keyword.</para></item>
        ///<item>style-src 'self' 'unsafe-inline';<para>Allows the use of inline style elements. You can implement a nonce-based CSP to remove the unsafe-inline keyword.</para></item>
        /// </list>
        /// </para>
        ///<para><br></br><b>X-Content-Type-Options</b>
        ///This is one of the headers which secures the content type of the data communicated.This header disables the wrong or malicious interpretation of Content-Type.
        ///This header has only one value “nosniff” i.e do not sniff the content type and choose the only content type specified by the application via Content-Type.</para>
        /// </summary>
        /// <param name="context"></param>
        public static void AddSecurityHeaders(this HttpContext context)
        {  ///"default-src 'self';"
            //var ContentSecurityPolicy = "default-src 'self'; img-src data: https: http:; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'; ";

            // context.Response.Headers.Add("X-XSS-Protection", "1; mode=block ");
            // context.Response.Headers.Add("Content-Security-Policy", ContentSecurityPolicy);
            // context.Response.Headers.Add("X-Content-Type-Options", "nosniff");


        }
        private class CustomHeaderSwaggerAttribute : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                operation.Parameters ??= new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "key",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "String"
                    },
                    Example = new OpenApiString("")
                });

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "lang",
                    In = ParameterLocation.Header,
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "String"
                    },
                    Example = new OpenApiString("EN")
                });
            }
        }
        /// <summary>
        /// Custom
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
        public static void AddControllersWithAddNewtonsoftJson(this WebApplicationBuilder builder)
        {

            builder.Services.AddControllers()
                .AddControllersAsServices()
               .AddNewtonsoftJson(options =>
               {
                   // To prevent "A possible object cycle was detected which is not supported" error
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

                   // To get our property names serialized in the first letter lowercased
                   //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                   options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
               }).ConfigureApiBehaviorOptions(options =>
               {
                   options.SuppressConsumesConstraintForFormFileParameters = true;
                   options.SuppressInferBindingSourcesForParameters = true;
                   options.SuppressModelStateInvalidFilter = true;
                   options.SuppressMapClientErrors = false;
                   options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
               });
            ////DO NOT CHANGE BELOW CODE ORIGINAL
            //builder.Services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //    options.SuppressInferBindingSourcesForParameters = false;
            //    options.SuppressMapClientErrors = true; 
            //});

        }

        /// <summary>
        /// Custom - Optimize upload file size more than 30MB
        /// </summary>
        /// <param name="services"></param>
        public static void AddUploadOptimizeStaticFiles(this WebApplicationBuilder builder)
        {
            ///Configure Upload file size
            //For application running on IIS:
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
            //For application running on Kestrel:
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });
            //3.Form's MultipartBodyLengthLimit
            builder.Services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // if don't set default value is: 128 MB
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }


        /// <summary>
        /// Custom - By default when you create a cookie in ASP.NET Core it is only applicable to that specific subdomain.
        ///<br>For example, a cookie created in subdomain.mydomain.com can not be shared with a second subdomain secondsubdomain.mydomain.com.
        ///</br><br>To change this behavior, you need to add the following code in Startup.ConfigureServices:</br>
        ///<code>services.ConfigureApplicationCookie(options =>
        ///{
        ///options.Cookie.Domain = ".mydomain.com";
        ///});</code>
        ///By specifying a common domain in the Cookie.Domain property, the cookie will be shared between 
        ///<br></br><b>addemo.gtechnosoft.com</b> and <b>apidemo.gtechnosoft.com</b>
        /// <br></br><seealso cref="https://bartwullems.blogspot.com/2021/09/aspnet-core-share-cookie-between.html"/>
        /// </summary>
        /// <param name="builder"></param>
        public static void AddDomainCookie(this WebApplicationBuilder builder, AppSettingsModel appSetting)
        {
            builder.Services.ConfigureApplicationCookie(options =>
            {
               
                options.Cookie.Domain = appSetting.AppSettings.Domain;
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                options.Cookie.Expiration = TimeSpan.FromDays(2);
            });
        }

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

        /// <summary>
        /// Custom -- <b>Step - 1</b> <br>Configure Add Antiforgery token </br>
        /// </summary>
        /// <param name="builder"></param>
        public static void AddAntiforgeyToken(this WebApplicationBuilder builder)
        {
            builder.Services.AddAntiforgery(
            options =>
            {
                // options.Cookie.Name = "X-XSRF-TOKEN";
                // options.FormFieldName = "__RequestVerificationToken";
                options.HeaderName = "X-XSRF-TOKEN";
                //options.SuppressXFrameOptionsHeader = true;

            });
            builder.Services.AddMvc(config => { config.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); });
        }
        /// <summary>
        /// Custom -- <b>Step - 2</b> <br>Use Antiforgery token</br>
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAntiforgeryTokens(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ValidateAntiForgeryTokenMiddleware>();
        }
        /// <summary>
        /// Custom - <b>Step - 3</b> <br>Append antiforgery token in HttpContext Response Cookie for admin api request</br>
        /// </summary>
        /// <param name="context"></param>
        public static void UseAntiforgeryTokenInResponse(this HttpContext context, string path, bool isDev, AppSettingsModel appSetting)
        {
            var tokens = context.RequestServices.GetRequiredService<IAntiforgery>();
            AntiforgeryTokenSet token = tokens.GetAndStoreTokens(context);

            //for cookie
            context.Response.Cookies.Append("X-XSRF-TOKEN", token.RequestToken, new CookieOptions()
            {
                Secure = true,
                Domain = appSetting.AppSettings.Domain,  
                HttpOnly = false,
                IsEssential = true,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict,
                //Expires = DateTime.Now.AddDays(2),
            });

            //context.Response.Cookies.Append("X-XSRF-TOKEN", token.RequestToken, new CookieOptions()
            //{
            //    HttpOnly = false,
            //    Secure = false,
            //    IsEssential = true,
            //    SameSite = SameSiteMode.Lax,
            //    Domain = isDev ? "localhost" : ".gtechnosoft.com",
            //    Expires = DateTime.Now.AddDays(2),
            //});

            //log every api call
            //if (isDev)
            //{
                Log.Information(path + "\nX-XSRF-TOKEN:" + token.RequestToken + "\n");
            //}
        }

        public static WebApplicationBuilder AddJsReport(this WebApplicationBuilder builder)
        {
            var localReporting = new LocalReporting()
                .UseBinary(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ?
                            jsreport.Binary.Linux.JsReportBinary.GetBinary() :
                            jsreport.Binary.JsReportBinary.GetBinary())
                .KillRunningJsReportProcesses()
                .Configure(cfg =>
                {
                    cfg.ReportTimeout = 60000;
                    cfg.Chrome = new ChromeConfiguration
                    {
                        Timeout = 60000,
                        Strategy = ChromeStrategy.ChromePool,
                        NumberOfWorkers = 3
                    };
                    cfg.HttpPort = 3000;
                    return cfg;
                })
                .AsUtility()
                .Create();

            builder.Services.AddJsReport(localReporting);

            return builder;
        }


        public static void DynamicDataMigration(this IApplicationBuilder app)
        {
            try
            {
                
                // migrate any database changes on startup (includes initial db creation)
                using (var scope = app.ApplicationServices.CreateScope())
                {

                    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dataContext.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Information("Dynamic Data Migration : " + ex.Message, Serilog.Events.LogEventLevel.Error);
            }
            
        }
    }
}
