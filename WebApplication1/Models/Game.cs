using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Game")]
    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game(Tournament t)
        {
            tournament = t;
        }

        public Game()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }


        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? date { get; set; }

        public long? bteam_id { get; set; }

        public long? rteam_id { get; set; }

        public long? tournament_id { get; set; }

        public long? winner_id { get; set; }

        public virtual Tournament tournament { get; set; }

        public virtual Team team { get; set; }

        public virtual Team team1 { get; set; }

        public virtual Team team2 { get; set; }
    }
}