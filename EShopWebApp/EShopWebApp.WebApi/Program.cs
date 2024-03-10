using EShopWebApp.Core.Contracts;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddApplicationServices(typeof(IPhotoService));
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(setup =>
{
    setup.AddPolicy("EShopWebApp", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://localhost:7298")
            .AllowAnyHeader()
            .AllowAnyMethod();


    });
});
 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors("EShopWebApp");

app.Run();
