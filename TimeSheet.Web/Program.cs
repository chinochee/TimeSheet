using Data;
using Services;
using Data.Persistence;
using Services.Configuration;
using Services.HttpClientService;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataLayer(builder.Configuration)
    .AddServicesLayer(builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.Configure<TableSettings>(builder.Configuration.GetSection(TableSettings.Settings));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(CacheSettings.Settings));
builder.Services.AddHttpClient<INamedBitcoinHttpClient, CoinDeskHttpClient>();
builder.Services.AddHttpClient<INamedBitcoinHttpClient, BlockchainInfoHttpClient>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

var app = builder.Build();

app.Migrate();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Login}")
    .RequireAuthorization();

app.Run();