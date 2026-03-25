using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TechShop.Data;
using TechShop.Models;
using TechShop.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Authentication & External Logins
var authBuilder = builder.Services.AddAuthentication();

// Google
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });
}

// Facebook
var facebookAppId = builder.Configuration["Authentication:Facebook:AppId"];
var facebookAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
if (!string.IsNullOrWhiteSpace(facebookAppId) && !string.IsNullOrWhiteSpace(facebookAppSecret))
{
    authBuilder.AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
        options.Fields.Add("name");
        options.Fields.Add("email");
        options.Scope.Clear(); // xóa scope mặc định
        options.Scope.Add("public_profile"); // chỉ dùng public_profile
    });
}

// 2FA Setup
builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
});

static bool IsAjaxRequest(HttpRequest request)
{
    // jQuery gửi X-Requested-With: XMLHttpRequest
    if (request.Headers.TryGetValue("X-Requested-With", out var v) && v == "XMLHttpRequest")
        return true;

    // Hoặc client muốn JSON
    var accept = request.Headers["Accept"].ToString();
    return accept.Contains("application/json", StringComparison.OrdinalIgnoreCase);
}
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = ctx =>
        {
            if (IsAjaxRequest(ctx.Request))
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            ctx.Response.Redirect(ctx.RedirectUri);
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = ctx =>
        {
            if (IsAjaxRequest(ctx.Request))
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
            ctx.Response.Redirect(ctx.RedirectUri);
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddHostedService<OrderCleanupService>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    await TechShop.Data.SeedData.Initialize(scope.ServiceProvider);
    await TechShop.Data.DataBooster.Boost(scope.ServiceProvider);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();