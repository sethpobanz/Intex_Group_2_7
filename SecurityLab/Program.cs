using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SecurityLab.Data;
using SecurityLab.Models;
using SecurityLab1.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var azureSqlConnectionString = configuration.GetConnectionString("AzureSqlConnection") ?? throw new InvalidOperationException("Connection string 'AzureSqlConnection' not found.");
services.AddDbContext<PobanzTestDbContext>(options =>
    options.UseSqlServer(azureSqlConnectionString)); // Use Azure SQL Server connection

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
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Users", "RequireAdminRole");
    options.Conventions.AuthorizeFolder("/Roles", "RequireAdminRole");
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

app.UseSession();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("pagenumandtype", "{legoType}/Page{pageNum}", new { Controller = "Home", Action = "Index" });
app.MapControllerRoute("page", "Page/{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("bookType", "{legoType}", new { Controller = "Home", Action = "Index", pageNum = 1 });
app.MapControllerRoute("pagination", "Legos/Page{pageNum}", new { Controller = "Home", Action = "Index", pageNum = 1 });



app.MapDefaultControllerRoute();
app.MapRazorPages();
app.Run();
