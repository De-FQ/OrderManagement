
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;
using Serilog;
using Utility.API;
using Web.Extensions;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddSession();
builder.Services.AddAuthenticationCookieService();
builder.Services.AddLocalizationService();
builder.Services.AddDependencyInjectionService(builder.Configuration);
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.AddSeriLogger();
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
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);


app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerCustomRoutes();

AppSettingsModel appSettings = builder.Configuration.Get<AppSettingsModel>();
if (appSettings == null)
    throw new Exception("message here");

if (appSettings.AppSettings.EnableRedirectToWwwRule)
{
    var options = new RewriteOptions();
    options.AddRedirectToHttps();
    options.Rules.Add(new RedirectToWwwRule());
    app.UseRewriter(options);
}

app.Run();
