using Tasko.Web.Service;
using Tasko.Web.Service.IService;
using Tasko.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ICouponService, CouponService>();
SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"] ?? string.Empty;
SD.OrderAPIBase = builder.Configuration["ServiceUrls:OrderAPI"] ?? string.Empty;
SD.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"] ?? string.Empty; ;
SD.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"] ?? string.Empty;
SD.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"] ?? string.Empty; ;
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
