using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
using MvcMovie.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ Switch from SQL Server to SQLite
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlite("Data Source=MvcMovie.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Apply migrations and seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MvcMovieContext>();
    context.Database.Migrate(); // Ensure DB + schema created

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Important for CSS/JS

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();
