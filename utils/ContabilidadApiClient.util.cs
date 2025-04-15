using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CRUDCxC.Utils;

public class ContabilidadApiClient
{
    private readonly HttpClient _httpClient;
    private string? _accessToken;

    private readonly string _baseUrl = Environment.GetEnvironmentVariable("BASE_CONTABILIDAD_URL") ?? throw new Exception("BASE_CONTABILIDAD_URL no configurado");
    private readonly string _user = Environment.GetEnvironmentVariable("CONTABILIDAD_API_USER") ?? throw new Exception("CONTABILIDAD_API_USER no configurado");
    private readonly string _password = Environment.GetEnvironmentVariable("CONTABILIDAD_API_PASSWORD") ?? throw new Exception("CONTABILIDAD_API_PASSWORD no configurado");
    private readonly int _auxiliarSystemId = int.Parse(Environment.GetEnvironmentVariable("CONTABILIDAD_AUX_ID") ?? "5");
    private readonly int _debitAccountId = int.Parse(Environment.GetEnvironmentVariable("CONTABILIDAD_DEBIT_ACCOUNT_ID") ?? "8");
    private readonly int _creditAccountId = int.Parse(Environment.GetEnvironmentVariable("CONTABILIDAD_CREDIT_ACCOUNT_ID") ?? "13");

    public ContabilidadApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> AuthenticateAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/Autenticacion/login");

        var credentials = new
        {
            email = _user,
            password = _password
        };

        request.Content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error autenticando con API Contabilidad: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonSerializer.Deserialize<JsonElement>(content);
        _accessToken = json.GetProperty("token").GetString();

        return _accessToken!;
    }

    public async Task<HttpClient> GetAuthenticatedClientAsync()
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            await AuthenticateAsync();
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        return _httpClient;
    }

    public async Task<bool> EnviarAsientoContableAsync(decimal total)
    {
        var client = await GetAuthenticatedClientAsync();

        var fechaAsiento = new DateTime(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            DateTime.UtcNow.Day,
            DateTime.UtcNow.Hour,
            DateTime.UtcNow.Minute,
            0, // segundos en 00
            DateTimeKind.Utc
        );


        var payload = new
        {
            sistemaAuxiliarId = _auxiliarSystemId,
            descripcion = $"Asiento de CxC correspondiente al periodo {DateTime.Now:yyyy-MM}",
            fechaAsiento = fechaAsiento.ToString("o"), // formato ISO 8601
            detalles = new[]
            {
            new { cuentaId = _creditAccountId, tipoMovimiento = "CR", montoAsiento = total },
            new { cuentaId = _debitAccountId, tipoMovimiento = "DB", montoAsiento = total }
        }
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"{_baseUrl}/EntradaContable", content);

        var responseBody = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"[DEBUG] API response: {response.StatusCode} | Body: {responseBody}");

        return response.IsSuccessStatusCode;
    }

}
