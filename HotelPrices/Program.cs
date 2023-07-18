using HotelPrices.Models.Context;
using HotelPrices.Models.Repository;
using HotelPrices.Models.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DB
builder.Services.AddDbContext<DbHotelPriceContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

//Serviços
builder.Services.AddTransient<IWebScraper, WebScraper>();
builder.Services.AddTransient<IHotelService, HotelService>();

//Repositories
builder.Services.AddTransient<IHotelPriceRepository, HotelPriceRepository>();

//interfaces
builder.Services.AddTransient<IDbHotelPriceContext, DbHotelPriceContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
