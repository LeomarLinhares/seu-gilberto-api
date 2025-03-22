using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PainelGilberto.Models
{
    [Table("UserRoundScores")]
    public class UserRoundScore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoundId { get; set; }

        [Required]
        public int Score { get; set; }

        [JsonIgnore]
        public Round Round { get; set; }
    }
}
