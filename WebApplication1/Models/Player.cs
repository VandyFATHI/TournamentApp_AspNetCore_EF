namespace WebApplication1.Models
{
    using System.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Player")]
    public partial class Player
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Player()
        {
            teams = new HashSet<Team>();
        }

        public Player(long teamId)
        {
            team_id = teamId;
            teams = new HashSet<Team>();

        }
      



        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public long team_id { get; set; }
        [ForeignKey("team_id")]

        public virtual Team team { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Team> teams { get; set; }
    }
}