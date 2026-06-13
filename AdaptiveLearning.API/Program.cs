using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AdaptiveLearning.API.Data;
using AdaptiveLearning.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddCors(o => o.AddPolicy("AllowAll", p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlite("Data Source=adaptive_learning.db"));

var jwtService = new JwtService(builder.Configuration);
builder.Services.AddSingleton(jwtService);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => { o.TokenValidationParameters = jwtService.GetValidationParams(); });

builder.Services.AddHttpClient<AiService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── ИНИЦИАЛИЗАЦИЯ БД ──────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    // Создаём admin если нет
    if (!db.Users.Any(u => u.Role == "Admin"))
    {
        var admin = new AdaptiveLearning.API.Models.User
        {
            Name         = "Администратор",
            Email        = "admin@adaptive.kz",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role         = "Admin",
        };
        db.Users.Add(admin);
        db.SaveChanges();
        db.LearningProfiles.Add(new AdaptiveLearning.API.Models.LearningProfile { UserId = admin.Id });
        db.SaveChanges();
    }

    // Seed данные (курсы + студенты)
    SeedData.Initialize(db);
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.Urls.Add("http://localhost:5000");
app.Run();
