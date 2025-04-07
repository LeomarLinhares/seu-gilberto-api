using PainelGilberto.DTOs;

namespace PainelGilberto.Interfaces
{
    public interface IBotService
    {
        string GetBotData();
        string SendMessage(string message);
        Task AnotarPontuacaoDaRodadaAtual(ScoreNotationDTO scoreNotation);
    }
}
