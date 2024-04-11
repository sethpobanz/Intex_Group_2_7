using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurityLab.Data;
using SecurityLab.Models;
using SecurityLab1.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"]
        ?? Environment.GetEnvironmentVariable("Authentication__Google__ClientId");
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"]
        ?? Environment.GetEnvironmentVariable("Authentication__Google__ClientSecret");
});

// Use Azure SQL Server connection
var azureSqlConnectionString = configuration.GetConnectionString("AzureSqlConnection")
    ?? Environment.GetEnvironmentVariable("AzureSqlConnection");
services.AddDbContext<PobanzTestDbContext>(options =>
    options.UseSqlServer(azureSqlConnectionString));

// Other service registrations remain the same...

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    // Define more policies as needed for different roles
});



builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductInterface, EFProductRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddScoped<IOrder2Repository, EFOrder2Repository>();

builder.Services.AddRazorPages();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Users", "RequireAdminRole");
    options.Conventions.AuthorizeFolder("/Roles", "RequireAdminRole");
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;

    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookieValue = "true";
});


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseSession();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapControllerRoute(
    name: "legoPagination",
    pattern: "Legos/Page{pageNum}",
    defaults: new { controller = "Home", action = "Index" }
);

app.MapControllerRoute(
    name: "orderPagination",
    pattern: "Orders/Page{pageNum}",
    defaults: new { controller = "Admin", action = "AdminOrdersView" }
);


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("pagenumandtype", "{legoType}/Page{pageNum}", new { Controller = "Home", Action = "Index" });
app.MapControllerRoute("page", "Page/{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("bookType", "{legoType}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("color", "{legoColor}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("legoPagination", "Legos/Page{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("orderPagination", "Orders/Page{pageNum}", new { Controller = "Admin", Action = "AdminOrdersView", pageNum = 1 });




app.MapDefaultControllerRoute();
app.MapRazorPages();
app.Run();
