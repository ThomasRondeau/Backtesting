﻿using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class OllamaService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _modelName;

    public OllamaService(string modelName = "llama3.2", string baseUrl = "http://localhost:11434")
    {
        _httpClient = new HttpClient();
        _baseUrl = baseUrl;
        _modelName = modelName;
    }

    public class GenerateRequest
    {
        public string Model { get; set; }
        public string Prompt { get; set; }
        public Dictionary<string, object> Options { get; set; }
    }

    public async Task<string> GenerateResponse(string prompt, bool stream = false)
    {
        var request = new
        {
            model = _modelName,
            prompt = prompt,
            stream = false  // Important : mettre explicitement à false pour une réponse unique
        };

        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/api/generate", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            // Extraire la réponse du JSON selon le format Ollama
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
            return responseObject.GetProperty("response").GetString();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erreur lors de l'appel à Ollama : {ex.Message}");
        }
    }

    // Méthode pour le streaming des réponses
    public async IAsyncEnumerable<string> GenerateStreamResponse(string prompt)
    {
        var request = new GenerateRequest
        {
            Model = _modelName,
            Prompt = prompt,
            Options = new Dictionary<string, object>
            {
                { "temperature", 0.7 },
                { "top_p", 0.9 },
                { "stream", true }
            }
        };

        var jsonContent = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/generate", content);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(line)) continue;

            var streamResponse = JsonSerializer.Deserialize<JsonElement>(line);
            if (streamResponse.TryGetProperty("response", out var responseToken))
            {
                yield return responseToken.GetString();
            }
        }
    }
}

// Exemple d'utilisation

/*
public class ExampleUsage
{
    public static async Task Main()
    {
        var ollama = new OllamaService("llama3.2");

        // Utilisation simple
        var response = await ollama.GenerateResponse("Explique moi le concept de récursivité");
        Console.WriteLine(response);

        // Utilisation avec streaming
        await foreach (var chunk in ollama.GenerateStreamResponse("Écris un poème sur le printemps"))
        {
            Console.Write(chunk);
        }
    }
}
*/