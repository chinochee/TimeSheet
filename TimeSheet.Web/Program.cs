using Data;
using Services;
using Data.Persistence;
using Services.Configuration;
using Services.BitcoinHttpClientService.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataLayer(builder.Configuration)
    .AddServicesLayer(builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.Configure<TableSettings>(builder.Configuration.GetSection(TableSettings.Settings));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(CacheSettings.Settings));
builder.Services.AddHttpClient<IBitcoinHttpClient, CoinDeskHttpClient>();
builder.Services.AddHttpClient<IBitcoinHttpClient, BlockchainInfoHttpClient>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();