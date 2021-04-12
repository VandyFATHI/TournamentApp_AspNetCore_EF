namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        date = c.DateTime(precision: 6, storeType: "datetime2"),
                        bteam_id = c.Long(),
                        rteam_id = c.Long(),
                        tournament_id = c.Long(),
                        winner_id = c.Long(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Team", t => t.rteam_id)
                .ForeignKey("dbo.Team", t => t.winner_id)
                .ForeignKey("dbo.Team", t => t.bteam_id)
                .ForeignKey("dbo.Tournament", t => t.tournament_id)
                .Index(t => t.bteam_id)
                .Index(t => t.rteam_id)
                .Index(t => t.tournament_id)
                .Index(t => t.winner_id);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 255, unicode: false),
                        nb_members = c.Int(),
                        captain_id = c.Long(),
                        tournament_id = c.Long(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Player", t => t.captain_id)
                .ForeignKey("dbo.Tournament", t => t.tournament_id)
                .Index(t => t.captain_id)
                .Index(t => t.tournament_id);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 255, unicode: false),
                        team_id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Team", t => t.team_id)
                .Index(t => t.team_id);
            
            CreateTable(
                "dbo.Tournament",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 255, unicode: false),
                        nb_participants = c.Int(),
                        description = c.String(maxLength: 255, unicode: false),
                        game = c.String(nullable: false, maxLength: 255, unicode: false),
                        start_date = c.DateTime(precision: 6, storeType: "datetime2"),
                        started = c.Binary(maxLength: 1, fixedLength: true),
                        team_size = c.Int(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Team", "tournament_id", "dbo.Tournament");
            DropForeignKey("dbo.Game", "tournament_id", "dbo.Tournament");
            DropForeignKey("dbo.Tournament", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Player", "team_id", "dbo.Team");
            DropForeignKey("dbo.Team", "captain_id", "dbo.Player");
            DropForeignKey("dbo.Game", "bteam_id", "dbo.Team");
            DropForeignKey("dbo.Game", "winner_id", "dbo.Team");
            DropForeignKey("dbo.Game", "rteam_id", "dbo.Team");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Tournament", new[] { "ApplicationUserId" });
            DropIndex("dbo.Player", new[] { "team_id" });
            DropIndex("dbo.Team", new[] { "tournament_id" });
            DropIndex("dbo.Team", new[] { "captain_id" });
            DropIndex("dbo.Game", new[] { "winner_id" });
            DropIndex("dbo.Game", new[] { "tournament_id" });
            DropIndex("dbo.Game", new[] { "rteam_id" });
            DropIndex("dbo.Game", new[] { "bteam_id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tournament");
            DropTable("dbo.Player");
            DropTable("dbo.Team");
            DropTable("dbo.Game");
        }
    }
}
