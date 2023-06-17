using Data;
using Data.Persistence;
using Services;
using Services.Configuration;
using Services.HttpClientService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataLayer(builder.Configuration)
    .AddServicesLayer(builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.Configure<TableSettings>(builder.Configuration.GetSection(TableSettings.Settings));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(CacheSettings.Settings));
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection(CookieSettings.Settings));
builder.Services.Configure<BaseUserCredis>(builder.Configuration.GetSection(BaseUserCredis.Credits));
builder.Services.AddHttpClient<INamedBitcoinHttpClient, CoinDeskHttpClient>();
builder.Services.AddHttpClient<INamedBitcoinHttpClient, BlockchainInfoHttpClient>();

var cookieSettings = builder.Configuration.GetSection(CookieSettings.Settings).Get(typeof(CookieSettings)) as CookieSettings;
builder.Services.AddAuthentication().AddCookie(cookieSettings.AuthenticationScheme);
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
        pattern: "{controller=TimeSheet}/{action=TimeSheets}")
.RequireAuthorization();

app.UseMiddleware<IMiddleware>();

if (app.Configuration.GetValue<bool>("NeedReInitialUsers"))
{
    var provider = app.Services.GetRequiredService<IServiceProvider>();

    using var scope = provider.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<IReInitialUsersService>();
    service.ReInitializeUsers();
}

app.Run();