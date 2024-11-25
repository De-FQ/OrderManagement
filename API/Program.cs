using API.Extensions;
using API.Hubs;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using Serilog;
using Utility.API;

var builder = WebApplication.CreateBuilder(args);

ExcelPackage.LicenseContext = LicenseContext.Commercial; // Use LicenseContext.Public if you are using a free license

//for common 
builder.AddScopedService();

builder.AddDatabaseContext();

// configure strongly typed settings objects  
var appSettingsSection = builder.Configuration.GetSection("appSettings");
builder.Services.Configure<AppSettingsModel>(appSettingsSection);

AppSettingsModel appSettings = builder.Configuration.Get<AppSettingsModel>();
if (appSettings == null) throw new Exception("message here"); //fail early 

 
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


builder.Services.TryAddSingleton(appSettings); 
builder.AddJwtAuthentication(appSettings);
builder.AddCors(appSettings);

//builder.AddJwtAuthorization();
builder.AddSwaggerService();
builder.AddMapper();

//for backend
builder.RegisterBackendService(); //BackendServiceExtensions.RegisterService(builder.Services);

//for frontend
builder.RegisterFrontendService();//builder.Services, builder.Configuration

builder.AddControllersWithAddNewtonsoftJson();
//memory cache
builder.Services.AddMemoryCache();
///add Data Protection on 5/oct/2023 (step-1)
builder.Services.AddDataProtection();



//add signalR
builder.Services.AddSignalR();

builder.AddUploadOptimizeStaticFiles();

builder.AddAntiforgeyToken();





builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});

builder.AddDomainCookie(appSettings);







//builder.Services.AddHsts(options =>
//{
//    options.Preload = true;
//    options.IncludeSubDomains = true;
//    options.MaxAge = TimeSpan.FromDays(365);
//    options.ExcludedHosts.Add("addemo.gtechnosoft.com");
//    options.ExcludedHosts.Add("apidemo.gtechnosoft.com");
//    options.ExcludedHosts.Add("localhost:7151"); 
//});
//add pdf generation
builder.Services.AddRazorTemplating();

builder.AddSeriLogger();

builder.AddJsReport();
var app = builder.Build();





app.UseSwagger();

if (app.Environment.IsDevelopment())
{
   // builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    app.UseDeveloperExceptionPage();

   // app.UseSwaggerUI();
}
else
{
   // app.UseSwaggerUI(); //it should be disabled in Production environment
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts();
}

if (appSettings.AppSettings.EnableSwagger)
{
    //to hide api move below code inside env.IsDevelopment()
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Management"));
}

app.UseHttpsRedirection();
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
app.UseCors();
app.UseRouting();
//app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.Use(next => context =>
{
 
    context.AddSecurityHeaders();  
    var path = context.Request.Path.Value;

    //if (path.ToLower().Contains("/api/images"))
    //{
    //    return Task.CompletedTask;
    //}
    //const string heartbit = "/api/heartbit";

    //if (path.ToLower().Contains("/api/heartbit")) {  || path.ToLower().Contains("/getfordatatable")

    //}
    if (path.ToLower().Contains(Utility.Helpers.Constants.Common.APIUrlForAntiforgeryToken) || path.ToLower().Contains("/addedit"))
    {

        //if user is not logged in, do not process api request
        if (!context.User.Claims.Any())
        {   //finished task 
            Log.Error("No user claims exists for the request: {0} = {1}", path, context.User.Identities.ToString());
            return Task.CompletedTask;
        }

        //add forgeryToken in API response (heart bit + add edit)
        context.UseAntiforgeryTokenInResponse(path,app.Environment.IsDevelopment(), appSettings);

        //ADMIN -- to inform api i am a real user
        if (path.ToLower().Contains(Utility.Helpers.Constants.Common.APIUrlForAntiforgeryToken))
        {
            //just return Antiforgery token to Admin and no further action is required
            context.Response.StatusCode = 200;
            
            return Task.CompletedTask;
        }
  
    }
     
     
    //process all received API request from Admin/Website
    return next(context);
});

app.UseAntiforgeryTokens();




app.DynamicDataMigration();


app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllers();
//use signalR
 //app.MapHub<UserHub>(Utility.Helpers.Constants.Common.APIForUrlHub);
 //app.MapHub<WebHub>(Utility.Helpers.Constants.Common.WehHubUrl);
app.Run();
 
