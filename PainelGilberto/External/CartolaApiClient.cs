using PainelGilberto.External;
using PainelGilberto.External.Models;
using System.Text.Json;

public class CartolaApiClient : ICartolaApiClient
{
    private readonly HttpClient _httpClient;

    public CartolaApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "PainelDoGilberto");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.BaseAddress = new Uri("https://api.cartola.globo.com/");
    }

    public async Task<CartolaStatusResponse> GetRodadaESeasonAtuaisAsync()
    {
        var response = await _httpClient.GetAsync("mercado/status");
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync(); // em vez de ReadAsString

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        var data = await JsonSerializer.DeserializeAsync<CartolaStatusResponse>(stream, options);

        if (data == null)
        {
            throw new Exception("Failed to deserialize Cartola status response");
        }

        return data;
    }
}
