using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace OllamaWebChat.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ChatController()
        {
            _httpClient = new HttpClient();
        }

        [HttpPost("explica")]
        public async Task<IActionResult> Explica([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Mensagem inválida.");

            var resposta = await ChamarOllamaAsync($"Explique o seguinte termo ou frase de forma simples: {request.Message}");
            return Ok(new { resposta });
        }

        private async Task<string> ChamarOllamaAsync(string prompt)
        {
            var url = "http://localhost:11434/api/generate"; // URL do Ollama REST API

            var dados = new
            {
                model = "llama3",
                prompt = prompt
            };

            var json = JsonSerializer.Serialize(dados);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resposta = await _httpClient.PostAsync(url, content);

            if (resposta.IsSuccessStatusCode)
            {
                var respostaJson = await resposta.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(respostaJson);
                var root = doc.RootElement;

                // Como o retorno vem em várias linhas com "response", vamos concatenar tudo:
                if (root.TryGetProperty("response", out var responseElement))
                {
                    return responseElement.GetString() ?? "Resposta vazia.";
                }
                else if (root.TryGetProperty("results", out var results) && results.GetArrayLength() > 0)
                {
                    var first = results[0];
                    if (first.TryGetProperty("text", out var text))
                        return text.GetString() ?? "Resposta vazia.";
                }

                return "Resposta no formato inesperado.";
            }
            else
            {
                return $"Erro na requisição: {resposta.StatusCode}";
            }
        }
    }

    public class ChatRequest
    {
        public string? Message { get; set; }
    }
}
