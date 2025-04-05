namespace PainelGilberto.DTOs
{
    public class DashboardDTO
    {
        public List<UserRankingDTO> Ranking { get; set; } = new();
        public List<UserCumulativeSeriesDTO> CumulativeSeries { get; set; } = new();
        public List<UserRoundScoreDTO> CurrentRoundScores { get; set; } = new();
    }

    public class UserRankingDTO
    {
        public string UserName { get; set; } = string.Empty;
        public decimal TotalSeasonPoints { get; set; }
        public string? MovementIndicator { get; set; }
    }

    public class UserCumulativeSeriesDTO
    {
        public string UserName { get; set; } = string.Empty;
        public List<CumulativeScorePointDTO> Points { get; set; } = new();
    }

    public class CumulativeScorePointDTO
    {
        public int RoundNumber { get; set; }
        public decimal CumulativeScore { get; set; }
    }

    public class UserRoundScoreDTO
    {
        public string UserName { get; set; } = string.Empty;
        public decimal RoundScore { get; set; }
    }
}
