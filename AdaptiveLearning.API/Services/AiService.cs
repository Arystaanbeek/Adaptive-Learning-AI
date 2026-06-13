using System.Text;
using System.Text.Json;

namespace AdaptiveLearning.API.Services;

public class AiService
{
    private readonly HttpClient _httpClient;
    private readonly string _aiServiceUrl;
    private readonly ILogger<AiService> _logger;

    public AiService(HttpClient httpClient, IConfiguration config, ILogger<AiService> logger)
    {
        _httpClient   = httpClient;
        _aiServiceUrl = config["AiService:BaseUrl"] ?? "http://localhost:8000";
        _logger       = logger;
    }

    public async Task<bool> IsHealthyAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_aiServiceUrl}/health");
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<AdaptResult?> PredictAdaptationAsync(AdaptInput input)
    {
        try
        {
            var json    = JsonSerializer.Serialize(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var res     = await _httpClient.PostAsync($"{_aiServiceUrl}/predict", content);
            if (!res.IsSuccessStatusCode) return null;
            var body    = await res.Content.ReadAsStringAsync();
            var opts    = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<AdaptResult>(body, opts);
        }
        catch (Exception ex)
        {
            _logger.LogError("AI service error: {e}", ex.Message);
            return null;
        }
    }
}

public class AdaptInput
{
    public double avg_score { get; set; }
    public double submission_rate { get; set; }
    public double total_clicks { get; set; }
    public double total_sessions { get; set; }
    public double ratio_video { get; set; }
    public double ratio_oucontent { get; set; }
    public double ratio_quiz { get; set; }
    public double dominant_style_enc { get; set; }
    public double unregistered { get; set; }
    public double num_of_prev_attempts { get; set; }
    public double studied_credits { get; set; }
    public double max_score { get; set; }
    public double min_score { get; set; }
    public double std_score { get; set; }
    public double submission_count { get; set; }
    public double score_trend { get; set; }
    public double q1_clicks { get; set; }
    public double q2_clicks { get; set; }
    public double q3_clicks { get; set; }
    public double q4_clicks { get; set; }
    public double early_vs_late { get; set; }
    public double early_registration { get; set; }
    public double ratio_video2 { get; set; }
    public double ratio_forumng { get; set; }
    public double ratio_resource { get; set; }
    public double ratio_page { get; set; }
    public double unique_resources { get; set; }
    public double avg_clicks_per_res { get; set; }
    public double revisit_rate { get; set; }
    public double study_regularity { get; set; }
    public double gender_enc { get; set; }
    public double region_enc { get; set; }
    public double highest_education_enc { get; set; }
    public double imd_band_enc { get; set; }
    public double age_band_enc { get; set; }
    public double disability_enc { get; set; }
}

public class AdaptResult
{
    public string Prediction { get; set; } = "";
    public Dictionary<string, double> Probabilities { get; set; } = new();
    public string LearningStyle { get; set; } = "";
    public string Recommendation { get; set; } = "";
}
