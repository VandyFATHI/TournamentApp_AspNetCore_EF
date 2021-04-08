namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        role_id = c.Int(nullable: false, identity: true),
                        role_name = c.String(),
                    })
                .PrimaryKey(t => t.role_id);
            
            AddColumn("dbo.Users", "role_id", c => c.Int());
            CreateIndex("dbo.Users", "role_id");
            AddForeignKey("dbo.Users", "role_id", "dbo.UserRole", "role_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "role_id", "dbo.UserRole");
            DropIndex("dbo.Users", new[] { "role_id" });
            DropColumn("dbo.Users", "role_id");
            DropTable("dbo.UserRole");
        }
    }
}
