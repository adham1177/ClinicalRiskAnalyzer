using System.Text;
using System.Text.Json;

namespace ClinicalRiskAnalyzer.Services
{
    public class GroqService : IAIService
    {
        private readonly string _apiKey;

        public GroqService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> AnalyzeAsync(string prompt)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var requestBody = new
            {
                model = "llama-3.3-70b-versatile",
                max_tokens = 1000,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            var responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);

            if (doc.RootElement.TryGetProperty("choices", out var choices))
            {
                return choices[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "No response";
            }

            return responseJson;
        }
    }
}