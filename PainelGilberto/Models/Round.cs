using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PainelGilberto.Models
{
    [Table("Rounds")]
    public class Round
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoundNumber { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartDateTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndDateTime { get; set; }

        [Required]
        public int SeasonId { get; set; }
        public Season Season { get; set; }
    }
}
