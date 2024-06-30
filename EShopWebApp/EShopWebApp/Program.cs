using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.Services;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using EShopWebApp.Infrastructure.DataBaseInitialization;
using EShopWebApp.Infrastructure.Extensions;
using EShopWebApp.Infrastructure.ModelBinders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["GOOGLE_CLIENT_ID"]!;
    googleOptions.ClientSecret = builder.Configuration["GOOGLE_CLIENT_SECRET"]!;
});
// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<DatabaseInitializer>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IEcontService, EcontService>();



builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddApplicationServices(typeof(IPhotoService));

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
    }); 

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
   

    var databaseInitializer = services.GetRequiredService<DatabaseInitializer>();

    await databaseInitializer.InitializeRolesAsync();
    await databaseInitializer.InitializeUsersAsync();
    await databaseInitializer.InitializeCategoriesAsync();
    await databaseInitializer.InitializeBrandsAsync();
    await databaseInitializer.InitializePhotosAsync();
    await databaseInitializer.InitializeProductsAsync();



}
StripeConfiguration.ApiKey = "sk_test_51PJxqG06TjIzi1CA5z0P6zBRIUVunCmpj2e8sHre5SdC99nV7X5wGlYkVucO9ETAF5QtP6FEEBFXKv0rUlojHN1j006PnezXHt";
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
    
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
