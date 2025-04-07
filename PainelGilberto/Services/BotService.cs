using PainelGilberto.DTOs;
using PainelGilberto.External;
using PainelGilberto.External.Models;
using PainelGilberto.Interfaces;
using PainelGilberto.Models;

namespace PainelGilberto.Services
{
    public class BotService : IBotService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoundScoreRepository _userRoundScoreRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly ICartolaApiClient _cartolaApiClient;
        public BotService(
            IUserRepository userRepository,
            IUserRoundScoreRepository userRoundScoreRepository,
            IRoundRepository roundRepository,
            ISeasonRepository seasonRepository,
            ICartolaApiClient cartolaApiClient)
        {
            _cartolaApiClient = cartolaApiClient;
            _userRoundScoreRepository = userRoundScoreRepository;
            _userRepository = userRepository;
            _roundRepository = roundRepository;
            _seasonRepository = seasonRepository;
        }
        public string GetBotData()
        {
            return "Bot data";
        }
        public string SendMessage(string message)
        {
            return $"Message sent: {message}";
        }
        public async Task AnotarPontuacaoDaRodadaAtual(ScoreNotationDTO scoreNotation)
        {
            try
            {
                User user = await _userRepository.GetUserByTelegramIdAsync(scoreNotation.TelegramUserId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                CartolaStatusResponse status = await _cartolaApiClient.GetRodadaESeasonAtuaisAsync();
                Season season = await _seasonRepository.GetSeasonByYear(status.Temporada);
                Round round = await _roundRepository.GetRoundByNumberAndSeasonAsync(status.RodadaAtual, season.Id);

                UserRoundScore userRoundScoreExisting = await _userRoundScoreRepository
                    .GetUserRoundScoreByUserIdAndRodadaAsync(user.Id, round.Id);

                if (userRoundScoreExisting != null)
                {
                    userRoundScoreExisting.Score = scoreNotation.Score;
                    _userRoundScoreRepository.Update(userRoundScoreExisting);
                    await _userRoundScoreRepository.SaveChangesAsync();
                }
                else
                {
                    UserRoundScore userRoundScore = new UserRoundScore
                    {
                        UserId = user.Id,
                        RoundId = round.Id,
                        Score = scoreNotation.Score
                    };

                    await _userRoundScoreRepository.AddAsync(userRoundScore);
                    await _userRoundScoreRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
