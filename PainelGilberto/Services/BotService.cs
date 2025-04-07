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
                List<User> allUsers = (await _userRepository.GetAllAsync()).ToList();
                User user = allUsers.Where(x => x.TelegramUserId == scoreNotation.TelegramUserId).FirstOrDefault();
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                CartolaStatusResponse status = await _cartolaApiClient.GetRodadaESeasonAtuaisAsync();
                Season season = await _seasonRepository.GetSeasonByYear(status.Temporada);
                Round round = await _roundRepository.GetRoundByNumberAndSeasonAsync(status.RodadaAtual, season.Id);

                List<UserRoundScore> allUserRoundScores = await _userRoundScoreRepository.GetbyRoundId(round.Id);

                // o score é o que o usuário envia como a pontuação que ele fez
                // o ranking score é calculado baseado na posição do usuário do ranking naquela rodada levando em conta todos os usuários

                List<Tuple<User, UserRoundScore>> userAndRoundScores = new List<Tuple<User, UserRoundScore>>();

                foreach (User userFromList in allUsers)
                {
                    UserRoundScore userRoundScoreFromList = allUserRoundScores.Where(x => x.UserId == userFromList.Id).FirstOrDefault();
                    if (userRoundScoreFromList != null)
                    {
                        userAndRoundScores.Add(new Tuple<User, UserRoundScore>(userFromList, userRoundScoreFromList));
                    }
                    else
                    {
                        userRoundScoreFromList = new UserRoundScore();
                        userRoundScoreFromList.UserId = userFromList.Id;
                        userRoundScoreFromList.RoundId = round.Id;
                        userRoundScoreFromList.Score = 0;
                        userRoundScoreFromList.RankingScore = 0;
                        userAndRoundScores.Add(new Tuple<User, UserRoundScore>(userFromList, userRoundScoreFromList));
                    }
                }

                // vamos ordenar as tuplas pelo score
                userAndRoundScores = userAndRoundScores.OrderByDescending(x => x.Item2.Score).ToList();

                // agora com as tuplas definidas, vamos calcular o ranking score
                // o ranking score é a posição do usuário no ranking daquela rodada - 1
                // agora vamos atualizar o score do usuário e o ranking score de todos
                foreach (Tuple<User, UserRoundScore> userAndRoundScore in userAndRoundScores)
                {
                    if (userAndRoundScore.Item1.Id == user.Id)
                    {
                        userAndRoundScore.Item2.Score = scoreNotation.Score;
                        userAndRoundScore.Item2.RankingScore = allUsers.Count() - userAndRoundScores.IndexOf(userAndRoundScore) - 1;
                    }
                    else
                    {
                        userAndRoundScore.Item2.Score = userAndRoundScore.Item2.Score;
                        userAndRoundScore.Item2.RankingScore = allUsers.Count() - userAndRoundScores.IndexOf(userAndRoundScore) - 1;
                    }
                    _userRoundScoreRepository.Update(userAndRoundScore.Item2);
                }

                // agora vamos tratar empates, ou seja, se dois ou mais usuários tiverem o mesmo score o ranking score deles deve ser o mesmo
                foreach (Tuple<User, UserRoundScore> userAndRoundScore in userAndRoundScores)
                {
                    List<Tuple<User, UserRoundScore>> usersWithSameScore = userAndRoundScores.Where(x => x.Item2.Score == userAndRoundScore.Item2.Score).ToList();
                    if (usersWithSameScore.Count() > 1)
                    {
                        foreach (Tuple<User, UserRoundScore> userWithSameScore in usersWithSameScore)
                        {
                            userWithSameScore.Item2.RankingScore = userAndRoundScore.Item2.RankingScore;
                            _userRoundScoreRepository.Update(userWithSameScore.Item2);
                        }
                    }
                }

                // agora vamos salvar as alterações
                await _userRoundScoreRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }
    }
}
