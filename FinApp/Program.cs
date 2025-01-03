using FinApp.Models;
using FinApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<ApplicationDbContext>(options => {
//    options.UseSqlServer(builder.Configuration["ConnectionStrings:FinAppDBConnection"]);
//});
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration["ConnectionStrings:AzureDBConnection"]);
});
builder.Services.Configure<IdentityOptions>(options => {
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUWXYZ1234567890";
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
});
builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.ExpireTimeSpan= TimeSpan.FromMinutes(10);
    options.SlidingExpiration = true;
});
builder.Services.AddScoped<BankAccountService>();
builder.Services.AddScoped<MonthlyExpenseService>();
builder.Services.AddScoped<CurrentMonthService>();
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Privacy}/{id?}");

app.Run();
