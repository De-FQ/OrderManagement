using Admin.Extensions; 
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Utility.LoggerService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();

//#region  For AntiForgery, Not tested
////https://www.aspsnippets.com/Articles/Implement-AntiForgery-Token-in-ASPNet-Web-API.aspx
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddAntiforgery(
//    options => {
//        options.Cookie.Name = "__RequestVerificationToken";
//        options.FormFieldName = "RequestVerificationToken";
//        options.HeaderName = "X-XSRF-TOKEN";
//        // options.SuppressXFrameOptionsHeader = true; 
//    });
//builder.Services.AddDisableAntiForgeryService();
//.AddNewtonsoftJson(options =>
//{
//    // To prevent "A possible object cycle was detected which is not supported" error
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

//    // To get our property names serialized in the first letter lowercased
//    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
//    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
//});
//builder.Services.addan.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
//#endregion  end For AntiForgery
// #if DEBUG
//   builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
//#endif

//it will add AntiForgery token inside all form within the admin project as below 
//<input name="X-CSRF-TOKEN-ASPNETCore7Template" type="hidden" value="CfDJ8KATOSXFw79IrKGJjENzO_WMpF0dbR0MleUGYIvwD5KaMi6k_cAnnzZauvBBAqmHjlbv7vaTb7yJQ1ueyodhL2-UR1Oj5qYNAUyz4o6lSwKOEVLjdrZVTjjMBk-x-4WYfAKa8-kBjrW0rQ_dq1vVqRdJ3SJG6pG1oBv5MMr8Ch0fiwP3icrXmMhfOHDt-4LZmg" />
//https://www.ryadel.com/en/asp-net-core-antiforgery-token-jquery-ajax-post-405/
//builder.Services.AddAntiforgery(options =>
//{
//    options.Cookie.Name = "X-CSRF-TOKEN";
//    //options.HeaderName = "X-CSRF-TOKEN";
//   // options.FormFieldName = "X-CSRF-TOKEN";
//});

//builder.Services.AddControllersWithViews(options =>
//     {
//         var policy = new AuthorizationPolicyBuilder()
//             .RequireAuthenticatedUser()
//             .Build();
//         options.Filters.Add(new AuthorizeFilter(policy));
//     });
//builder.Services.AddAuthorization(options =>
//{
//          options.AddPolicy("ElevatedRights", policy =>
//          policy.RequireRole("Administrator", "PowerUser", "BackupAdministrator", "EmployeeOnly"));
//});

//Add Authentication Cookie
//https://brokul.dev/authentication-cookie-lifetime-and-sliding-expiration

builder.Services.AddAuthenticationCookieService();
builder.Services.AddLocalizationService();
builder.Services.AddDependencyInjectionService(builder.Configuration);


//builder.Services.AddSessionService();
//builder.Services.AddAuthorizationPolicy();

//    pipeline =>
//{
//    //var provider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Environment.cu);
//    pipeline.MinifyJsFiles("/developer/js/app/app.js");
//    //.AddJavaScriptBundle("/developer/js/*.*");
//    //.UseFileProvider(provider);
//});

//builder.Services.ConfigureApplicationCookie(options =>
//{ 
//    options.Cookie.Domain = builder.Environment.IsDevelopment() ? "localhost" : ".gtechnosoft.com";
//    options.Cookie.HttpOnly = false;
//    ////options.Cookie.s = builder.Environment.IsDevelopment() ? false : true;
//    options.Cookie.IsEssential = true;
//     options.Cookie.SameSite = SameSiteMode.Strict;
//     options.Cookie.Expiration = TimeSpan.FromDays(2);
//});
Utility.LoggerService.AppSeriLog.Configure(builder);

var app = builder.Build();
//if (!app.Environment.IsDevelopment())
//{
//    builder.Services.AddWebOptimizer();
//}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    //app.UseDatabaseErrorPage();
}

////ReWrite Url --> user type new it will redirect to home/privacy page
//var newUrl = new RewriteOptions().AddRewrite("new", "home/privacy",false);
//app.UseRewriter(newUrl);

app.UseHttpsRedirection();
//if (!app.Environment.IsDevelopment())
//{
//    //add app.UseWebOptimizer() to the Configure method anywhere before app.UseStaticFiles (if present),  
//    //builder.Services.AddWebOptimizer();
//    app.UseWebOptimizer();
//}

app.UseStaticFiles();
app.UseRouting();

////start for localized, if you forget, 2 language will show up in view 
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
//end for localized

//for cookie encryption
//app.UseCookiePolicy( new CookiePolicyOptions {MinimumSameSitePolicy = SameSiteMode.Strict});
 
app.UseAuthentication();
app.UseAuthorization();

 //app.UseSessionService(); 

app.MapControllerCustomRoutes();

////add cart item
//app.MapControllerRoute(
//       name: "MyLogin",
//       pattern: "MyLogin",
//       defaults: new { controller = "Account", action = "MyLogin" });


app.Run();
