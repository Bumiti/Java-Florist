using JavaFloristClient.Data;
using JavaFloristClient.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JavaFloristClientContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JavaFloristClientContext") ?? throw new InvalidOperationException("Connection string 'JavaFloristClientContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("MyApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5026/");
    /*client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));*/
});
/*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Accounts/Login"; // Customize the login URL
            options.AccessDeniedPath = "/Accounts/AccessDenied"; // Customize the access denied URL
        });
builder.Services.AddControllersWithViews();*/
/*builder.Services.AddAuthentication("MyCookieAuth")
        .AddCookie("MyCookieAuth", options =>
        {
            options.LoginPath = "/Accounts/Login"; // Customize the login URL
            options.AccessDeniedPath = "/Accounts/AccessDenied"; // Customize the access denied URL
        });
// Add authorization services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});*/
/*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "access_token";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Accounts/Login";
            });*/
builder.Services.AddTransient<IJavaFloristClientService, JavaFloristClientService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=DashBoard}/{id?}");

app.Run();
