using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("UserRole")]
    public partial class UserRole
    {
        public UserRole()
        {

        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int role_id { get; set; }

        public string role_name { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}