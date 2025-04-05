using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PainelGilberto.Data;
using PainelGilberto.DTOs;
using PainelGilberto.Models;

namespace PainelGilberto.Services
{
    public class GeneralInfoService : IGeneralInfoService
    {
        private readonly ApplicationDbContext _context;

        public GeneralInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Exemplo simples que retorna Rounds + Season
        public IEnumerable<Round> GetRoundsBySeason()
        {
            return _context.Rounds
                           .Include(r => r.Season)
                           .ToList();
        }

        // Método que retorna todos os dados do dashboard
        public DashboardDTO GetAllDashboardData(int seasonId)
        {
            // 1) Identificar o "round atual" (o de maior RoundNumber na Season)
            var currentRoundNumber = _context.Rounds
                .Where(r => r.SeasonId == seasonId)
                .Select(r => (int?)r.RoundNumber) // Converte para int? (nullable)
                .Max()                             // Se não houver registros, retorna null
                ?? 0;

            // 2) Carregar todos os usuários (vamos exibir todos)
            var users = _context.Users.ToList();

            // 3) Carregar todos os UserRoundScores da temporada
            var userRoundScores = _context.UserRoundScores
                .Include(urs => urs.Round)
                .Where(urs => urs.Round.SeasonId == seasonId)
                .ToList();

            // 4) Calcular pontuação total por usuário
            var totalSeasonPoints = userRoundScores
                .GroupBy(urs => urs.UserId)
                .Select(g => new { UserId = g.Key, Total = g.Sum(x => x.Score) })
                .ToList();

            // 5) Calcular pontuação total até a rodada anterior (para comparar)
            var previousRoundNumber = currentRoundNumber > 1 ? currentRoundNumber - 1 : 0;
            var totalUpToPreviousRound = userRoundScores
                .Where(urs => urs.Round.RoundNumber <= previousRoundNumber)
                .GroupBy(urs => urs.UserId)
                .Select(g => new { UserId = g.Key, Total = g.Sum(x => x.Score) })
                .ToList();

            // Montar ranking atual e ranking anterior (por total de pontos)
            var currentRank = totalSeasonPoints
                .OrderByDescending(x => x.Total)
                .Select((x, index) => new { x.UserId, Rank = index + 1 })
                .ToList();

            var previousRank = totalUpToPreviousRound
                .OrderByDescending(x => x.Total)
                .Select((x, index) => new { x.UserId, Rank = index + 1 })
                .ToList();

            // Dicionários para lookup rápido
            var dictCurrentTotals = totalSeasonPoints.ToDictionary(x => x.UserId, x => x.Total);
            var dictPreviousTotals = totalUpToPreviousRound.ToDictionary(x => x.UserId, x => x.Total);
            var dictCurrentRanks = currentRank.ToDictionary(x => x.UserId, x => x.Rank);
            var dictPreviousRanks = previousRank.ToDictionary(x => x.UserId, x => x.Rank);

            // 6) Montar o UserRankingDTO (ordenado por pontuação atual decrescente)
            var userRankingDTOs = users.Select(u =>
            {
                var currentTotal = dictCurrentTotals.GetValueOrDefault(u.Id, 0);
                var prevRank = dictPreviousRanks.GetValueOrDefault(u.Id, users.Count);
                var currRank = dictCurrentRanks.GetValueOrDefault(u.Id, users.Count);

                // Se a posição subiu, "up"; se desceu, "down"; se igual, null
                string? movementIndicator = null;
                if (prevRank > currRank) movementIndicator = "up";
                else if (prevRank < currRank) movementIndicator = "down";

                return new UserRankingDTO
                {
                    UserName = $"{u.FirstName} {u.LastName}",
                    TotalSeasonPoints = currentTotal,
                    MovementIndicator = movementIndicator
                };
            })
            .OrderByDescending(x => x.TotalSeasonPoints)
            .ToList();

            // 7) Montar as séries cumulativas
            // 7.1) Carregar as rodadas da temporada (ordenadas)
            var rounds = _context.Rounds
                .Where(r => r.SeasonId == seasonId)
                .OrderBy(r => r.RoundNumber)
                .ToList();

            var cumulativeSeries = new List<UserCumulativeSeriesDTO>();

            foreach (var user in users)
            {
                var userScores = userRoundScores.Where(x => x.UserId == user.Id).ToList();
                decimal runningTotal = 0;
                var pointsList = new List<CumulativeScorePointDTO>();

                // Para cada rodada, soma a pontuação e guarda o total acumulado
                foreach (var round in rounds)
                {
                    decimal roundScore = userScores
                        .Where(x => x.RoundId == round.Id)
                        .Sum(x => x.Score);

                    runningTotal += roundScore;

                    pointsList.Add(new CumulativeScorePointDTO
                    {
                        RoundNumber = round.RoundNumber,
                        CumulativeScore = runningTotal
                    });
                }

                cumulativeSeries.Add(new UserCumulativeSeriesDTO
                {
                    UserName = $"{user.FirstName} {user.LastName}",
                    Points = pointsList
                });
            }

            // 8) Montar pontuação da rodada atual
            // Se não há rounds, essa lista fica vazia
            var currentRoundScores = new List<UserRoundScoreDTO>();
            if (currentRoundNumber > 0)
            {
                // Pega o ID da rodada atual
                var currentRound = rounds.FirstOrDefault(r => r.RoundNumber == currentRoundNumber);
                if (currentRound != null)
                {
                    // Filtra só os scores da rodada atual
                    var ursCurrentRound = userRoundScores
                        .Where(x => x.RoundId == currentRound.Id)
                        .ToList();

                    // Gera a lista para cada usuário
                    currentRoundScores = users.Select(u =>
                    {
                        decimal score = ursCurrentRound
                            .Where(x => x.UserId == u.Id)
                            .Sum(x => x.Score);

                        return new UserRoundScoreDTO
                        {
                            UserName = $"{u.FirstName} {u.LastName}",
                            RoundScore = score
                        };
                    })
                    .ToList();
                }
            }

            // 9) Retornar o DTO completo
            var dashboardDTO = new DashboardDTO
            {
                Ranking = userRankingDTOs,
                CumulativeSeries = cumulativeSeries,
                CurrentRoundScores = currentRoundScores
            };

            return dashboardDTO;
        }
    }
}
