using AdaptiveLearning.API.Data;
using AdaptiveLearning.API.Models;
using AdaptiveLearning.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace AdaptiveLearning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;
    private readonly IConfiguration _config;
    private readonly HttpClient _http;

    public AuthController(AppDbContext db, JwtService jwt, IConfiguration config, IHttpClientFactory factory)
    {
        _db = db;
        _jwt = jwt;
        _config = config;
        _http = factory.CreateClient();
    }

    // POST api/auth/register
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name) ||
            string.IsNullOrWhiteSpace(req.Email) ||
            string.IsNullOrWhiteSpace(req.Password))
            return BadRequest(new { error = "Заполните все поля" });

        if (req.Password.Length < 6)
            return BadRequest(new { error = "Пароль минимум 6 символов" });

        if (await _db.Users.AnyAsync(u => u.Email == req.Email.ToLower()))
            return BadRequest(new { error = "Email уже зарегистрирован" });

        var user = new User
        {
            Name         = req.Name.Trim(),
            Email        = req.Email.ToLower().Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Role         = "Student",
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Создаём пустой профиль обучения
        _db.LearningProfiles.Add(new LearningProfile { UserId = user.Id });
        await _db.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            Token  = _jwt.GenerateToken(user),
            Name   = user.Name,
            Email  = user.Email,
            Role   = user.Role,
            UserId = user.Id,
        });
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == req.Email.ToLower());

        if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { error = "Неверный email или пароль" });

        return Ok(new AuthResponse
        {
            Token  = _jwt.GenerateToken(user),
            Name   = user.Name,
            Email  = user.Email,
            Role   = user.Role,
            UserId = user.Id,
        });
    }

    // GET api/auth/me  (проверка токена)
    [HttpGet("me")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<ActionResult> Me()
    {
        var userId = int.Parse(User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return NotFound();
        return Ok(new { user.Id, user.Name, user.Email, user.Role });
    }

    // GET api/auth/google/login — редирект на Google
    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        var clientId = _config["Google:ClientId"];
        var redirectUri = "http://localhost:5000/api/auth/google/callback";
        var scope = "openid email profile";
        var url = $"https://accounts.google.com/o/oauth2/v2/auth" +
                  $"?client_id={clientId}" +
                  $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
                  $"&response_type=code" +
                  $"&scope={Uri.EscapeDataString(scope)}";
        return Redirect(url);
    }

    // GET api/auth/google/callback — получаем токен от Google
    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback([FromQuery] string code)
    {
        var clientId = _config["Google:ClientId"];
        var clientSecret = _config["Google:ClientSecret"];
        var redirectUri = "http://localhost:5000/api/auth/google/callback";

        // Меняем code на токен
        var tokenRes = await _http.PostAsync("https://oauth2.googleapis.com/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = clientId!,
                ["client_secret"] = clientSecret!,
                ["redirect_uri"] = redirectUri,
                ["grant_type"] = "authorization_code",
            }));
        var tokenJson = await tokenRes.Content.ReadAsStringAsync();
        var tokenDoc = JsonDocument.Parse(tokenJson);
        var accessToken = tokenDoc.RootElement.GetProperty("access_token").GetString();

        // Получаем данные пользователя
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        var userRes = await _http.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
        var userJson = await userRes.Content.ReadAsStringAsync();
        var userDoc = JsonDocument.Parse(userJson);
        var email = userDoc.RootElement.GetProperty("email").GetString()!;
        var name = userDoc.RootElement.GetProperty("name").GetString()!;

        // Ищем или создаём пользователя
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            user = new User
            {
                Name = name,
                Email = email,
                Role = "Student",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            _db.LearningProfiles.Add(new LearningProfile { UserId = user.Id });
            await _db.SaveChangesAsync();
        }

        var token = _jwt.GenerateToken(user);
        // Передаём токен на фронтенд через URL
        return Redirect($"/auth-callback.html?token={token}&name={Uri.EscapeDataString(user.Name)}&email={Uri.EscapeDataString(user.Email)}&role={user.Role}&userId={user.Id}");
    }
}
