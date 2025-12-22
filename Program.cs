using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Star_Security.Data;
using Star_Security.Models;
using Star_Security.Models.Config;
using Star_Security.Services;
using Star_Security.Services.Email;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("dbcs");
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(conn)
);

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.@";
    }
)
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
    );

builder.Services.AddScoped<IEmailSender, EmailSender>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedService.SeedRolesAndAdmin(services);
}

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
