using GroupProject.Models; // Додано
using Microsoft.EntityFrameworkCore; // Додано

var builder = WebApplication.CreateBuilder(args);

// 💾 Реєструємо ApplicationDbContext з тимчасовою InMemory базою
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("HotelDb"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
