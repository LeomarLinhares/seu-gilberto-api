using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PainelGilberto.Models
{
    public class Round
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoundNumber { get; set; }

        [ForeignKey("Season")]
        public int SeasonId { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public Season Season { get; set; }
        public ICollection<UserRoundScore> UserRoundScores { get; set; } = new List<UserRoundScore>();
    }
}
