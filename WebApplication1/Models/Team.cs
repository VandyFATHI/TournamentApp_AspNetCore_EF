using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Team")]
    public partial class Team
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Team()
        {
            //red team
            games = new HashSet<Game>();
            //winner
            games1 = new HashSet<Game>();
            //blue team
            games2 = new HashSet<Game>();
            players = new HashSet<Player>();
        }

        public Team(long? tournamentId)
        {
            tournament_id = tournamentId;
            games = new HashSet<Game>();
            games1 = new HashSet<Game>();
            games2 = new HashSet<Game>();
            players = new HashSet<Player>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public int? nb_members { get; set; }

        public long? captain_id { get; set; }

        public long? tournament_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> games { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> games1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Game> games2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Player> players { get; set; }

        public virtual Player player { get; set; }

        public virtual Tournament tournament { get; set; }
    }
}