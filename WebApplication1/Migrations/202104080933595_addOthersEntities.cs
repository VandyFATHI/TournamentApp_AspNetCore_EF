namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOthersEntities : DbMigration
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
                .ForeignKey("dbo.Users", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Team", "tournament_id", "dbo.Tournament");
            DropForeignKey("dbo.Game", "tournament_id", "dbo.Tournament");
            DropForeignKey("dbo.Tournament", "ApplicationUserId", "dbo.Users");
            DropForeignKey("dbo.Player", "team_id", "dbo.Team");
            DropForeignKey("dbo.Team", "captain_id", "dbo.Player");
            DropForeignKey("dbo.Game", "bteam_id", "dbo.Team");
            DropForeignKey("dbo.Game", "winner_id", "dbo.Team");
            DropForeignKey("dbo.Game", "rteam_id", "dbo.Team");
            DropIndex("dbo.Tournament", new[] { "ApplicationUserId" });
            DropIndex("dbo.Player", new[] { "team_id" });
            DropIndex("dbo.Team", new[] { "tournament_id" });
            DropIndex("dbo.Team", new[] { "captain_id" });
            DropIndex("dbo.Game", new[] { "winner_id" });
            DropIndex("dbo.Game", new[] { "tournament_id" });
            DropIndex("dbo.Game", new[] { "rteam_id" });
            DropIndex("dbo.Game", new[] { "bteam_id" });
            DropTable("dbo.Tournament");
            DropTable("dbo.Player");
            DropTable("dbo.Team");
            DropTable("dbo.Game");
        }
    }
}
