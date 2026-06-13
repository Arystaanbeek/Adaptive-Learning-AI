using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Text.Json;

namespace AdaptiveLearning.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILogger<AiController> _logger;
    private readonly HttpClient _http;

    public AiController(IConfiguration config, ILogger<AiController> logger, IHttpClientFactory factory)
    {
        _config = config;
        _logger = logger;
        _http = factory.CreateClient();
    }

    [HttpPost("ask")]
    public async Task<ActionResult> Ask([FromBody] AiAskRequest req)
    {
        var geminiKey = _config["Gemini:ApiKey"];

        _logger.LogInformation("Gemini key starts with: {k}", geminiKey?.Substring(0, Math.Min(10, geminiKey?.Length ?? 0)));

        if (string.IsNullOrEmpty(geminiKey) || geminiKey == "YOUR_GEMINI_KEY")
            return Ok(new { answer = FallbackAnswer(req.Question) });

        try
        {
            var prompt = $"""
    Ты — дружелюбный AI-наставник образовательной платформы AdaptiveLearn.
    Твоя задача: мотивировать и помогать студенту расти.
    
    Правила:
    - НИКОГДА не упоминай провал, риск, плохие оценки
    - Всегда говори позитивно: "можно улучшить", "следующий шаг", "твоя сильная сторона"
    - Давай конкретные советы по теме
    - Отвечай кратко (2-4 предложения) на русском языке
    
    Тема курса: {req.Topic}
    Вопрос студента: {req.Question}
    """;
            var payload = new { contents = new[] { new { parts = new[] { new { text = prompt } } } } };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={geminiKey}";

            var res = await _http.PostAsync(url, content);
            var body = await res.Content.ReadAsStringAsync();

            _logger.LogInformation("Gemini status: {s}, body: {b}", res.StatusCode, body[..Math.Min(300, body.Length)]);

            if (!res.IsSuccessStatusCode)
                return Ok(new { answer = $"Ошибка Gemini {res.StatusCode}: {body[..Math.Min(200, body.Length)]}" });

            var doc = JsonDocument.Parse(body);
            var answer = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text").GetString();

            return Ok(new { answer = answer ?? FallbackAnswer(req.Question) });
        }
        catch (Exception ex)
        {
            _logger.LogError("Gemini exception: {e}", ex.Message);
            return Ok(new { answer = "Ошибка соединения с Gemini: " + ex.Message });
        }
    }

    string FallbackAnswer(string question) =>
        $"По теме '{question}' рекомендую обратиться к материалам курса. Gemini API ключ не настроен.";
}

public class AiAskRequest
{
    public string Question { get; set; } = "";
    public string Topic { get; set; } = "";
}