using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant d'autres propriétés à votre classe ApplicationUser. Pour en savoir plus, consultez https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string RoleId { get; set; }
        //[ForeignKey("RoleId")]
        //public userManager UserManager { get; set; }
        //public ICollection<Tournament> UserTournaments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }

    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=DBmodel", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<ApplicationUser>().ToTable("Users");

            //modelBuilder.Entity<ApplicationUser>()
              //  .HasOptional(a => a.UserTournaments);

            modelBuilder.Entity<Game>()
                .Property(e => e.date)
                .HasPrecision(6);

            modelBuilder.Entity<Player>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Player>()
                .HasMany(e => e.teams)
                .WithOptional(e => e.player)
                .HasForeignKey(e => e.captain_id);

            modelBuilder.Entity<Team>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.games)
                .WithOptional(e => e.team)
                .HasForeignKey(e => e.rteam_id);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.games1)
                .WithOptional(e => e.team1)
                .HasForeignKey(e => e.winner_id);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.games2)
                .WithOptional(e => e.team2)
                .HasForeignKey(e => e.bteam_id);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.players)
                .WithRequired(e => e.team)
                .HasForeignKey(e => e.team_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tournament>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Tournament>()
                .Property(e => e.game)
                .IsUnicode(false);

            //modelBuilder.Entity<tournament>()
            //  .Property(e => e._private)
            //.IsFixedLength();

            modelBuilder.Entity<Tournament>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Tournament>()
                .Property(e => e.start_date)
                .HasPrecision(6);

            modelBuilder.Entity<Tournament>()
                .Property(e => e.started)
                .IsFixedLength();

            modelBuilder.Entity<Tournament>()
                .HasMany(e => e.games)
                .WithOptional(e => e.tournament)
                .HasForeignKey(e => e.tournament_id);

            modelBuilder.Entity<Tournament>()
                .HasMany(e => e.teams)
                .WithOptional(e => e.tournament)
                .HasForeignKey(e => e.tournament_id);

        }
    }

    public class userManager : UserManager<ApplicationUser>
    {
        public userManager(IUserStore<ApplicationUser> store) : base(store)
        {

        }

        public static userManager Create(IdentityFactoryOptions<userManager> options, IOwinContext context)
        {
            ApplicationDbContext db = context.Get<ApplicationDbContext>();
            userManager manager = new userManager(new UserStore<ApplicationUser>(db));

            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };
            return manager;
        }
        
    }
}