using PainelGilberto.External.Models;

namespace PainelGilberto.External
{
    public interface ICartolaApiClient
    {
        Task<CartolaStatusResponse> GetRodadaESeasonAtuaisAsync();
    }

}
