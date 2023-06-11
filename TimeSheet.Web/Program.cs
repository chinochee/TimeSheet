using Data;
using Data.Persistence;
using Services;
using Services.Configuration;
using Services.HttpClientService;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataLayer(builder.Configuration)
    .AddServicesLayer(builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.Configure<TableSettings>(builder.Configuration.GetSection(TableSettings.Settings));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(CacheSettings.Settings));
builder.Services.Configure<InitialUsersSettings>(builder.Configuration.GetSection(InitialUsersSettings.Settings));
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection(CookieSettings.Settings));
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

app.Use(TimeWorkRequestLogger);

var initialUserSettings = app.Configuration.GetSection(InitialUsersSettings.Settings).Get(typeof(InitialUsersSettings)) as InitialUsersSettings;
if (initialUserSettings.NeedReInitialUsers)
{
    var provider = app.Services.GetRequiredService<IServiceProvider>();

    using var scope = provider.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<IReInitialUsersService>();
    service.ReInitializeUsers();
}

app.Run();

async Task TimeWorkRequestLogger(HttpContext context, Func<Task> next)
{
    var startTime = Stopwatch.GetTimestamp();

    await next.Invoke();

    var elapsedTime = Stopwatch.GetElapsedTime(startTime);

    app.Logger.LogInformation("Request completed in {0} milliseconds", elapsedTime.Milliseconds);
}