using ASPSession.Security;
using ASPSession.Connectors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IConnector, ShpCrtConnector>();
builder.Services.AddSingleton<ISecurity, PvtSecurity>();

builder.Services.AddDbContext<EFConnector>(opt =>
{
    string db_conn = @"Server=localhost;Database=ShoppingCartDB; Integrated Security = true";
    opt.UseLazyLoadingProxies().UseSqlServer(db_conn);
  
});

builder.Services.AddControllersWithViews();





builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); //logs out if user AFK too long 
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Main}/{id?}");

app.Run();
