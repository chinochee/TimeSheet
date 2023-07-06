using Data;
using Data.Persistence;
using Services;
using Services.Configuration;
using Services.HttpClientService;

var location = System.Reflection.Assembly.GetExecutingAssembly().Location;

var indexTimeSheet = location.IndexOf("TimeSheet");

var dirInfo = new DirectoryInfo(location.Remove(indexTimeSheet + "TimeSheet".Length));
dirInfo.CreateSubdirectory(@"TimeSheetDataBaseHolder");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDataLayer(builder.Configuration)
    .AddServicesLayer(builder.Configuration);

builder.Services.AddRazorPages();
builder.Services.Configure<TableSettings>(builder.Configuration.GetSection(TableSettings.Settings));
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection(CacheSettings.Settings));
builder.Services.Configure<Permissions>(builder.Configuration.GetSection(Permissions.Key));
builder.Services.Configure<BaseUserCredits>(builder.Configuration.GetSection(BaseUserCredits.Credits));
builder.Services.AddHttpClient<INamedBitcoinHttpClient, CoinDeskHttpClient>();
builder.Services.AddHttpClient<INamedBitcoinHttpClient, BlockchainInfoHttpClient>();

builder.Services.AddAuthentication().AddCookie(CookieSettingsConstant.AuthenticationScheme);
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