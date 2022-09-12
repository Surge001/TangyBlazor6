using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Tangy.Business.Repository;
using Tangy.Business.Repository.IRepository;
using Tangy.DataAccess.Data;
using TangyWeb.Server.Data;
using TangyWeb.Server.Service;
using TangyWeb.Server.Service.IService;
using Syncfusion.Blazor;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var builder = WebApplication.CreateBuilder(args);

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2VVhiQlFadVlJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdkxjWn5adXFWQGdcUEE=;NzA5OTUxQDMyMzAyZTMyMmUzMGw3SURGenhuTEwySytZVUZ0MlRoeFg5SUpja1lMaEdWblhGd2xISE15Tnc9");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor(options => { });


builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductPriceRepository, ProductPriceRepository>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

SeedDatabase(); //< -- Creates admin system account for application administration.

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseAuthentication(); ;
app.UseAuthorization();
app.Run();


/// <summary>
/// Executes database seeding logic which happens in check whether there is admin account
/// already created or not, and if not, it creates one automatically.
/// </summary>
void SeedDatabase()
{
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IDbInitializer dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}