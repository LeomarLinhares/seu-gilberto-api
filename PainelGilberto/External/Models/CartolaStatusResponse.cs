using System.Text.Json.Serialization;

namespace PainelGilberto.External.Models
{
    public class CartolaStatusResponse
    {
        [JsonPropertyName("rodada_atual")]
        public int RodadaAtual { get; set; }

        [JsonPropertyName("temporada")]
        public int Temporada { get; set; }
    }
}
