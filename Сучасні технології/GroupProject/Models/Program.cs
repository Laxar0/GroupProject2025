namespace GroupProject.Models
{
    public class Program
    {
        var builder = WebApplication.CreateBuilder(args);

        // Додаємо контекст БД
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();
    }
}
