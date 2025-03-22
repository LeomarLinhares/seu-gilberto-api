using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PainelGilberto.Models
{
    [Table("Seasons")]
    public class Season
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Year { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [JsonIgnore]
        public ICollection<Round> Rounds { get; set; } = new List<Round>();
    }
}
