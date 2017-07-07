namespace OnlineContestSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contestants",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Nation = c.Int(nullable: false),
                        States = c.Int(nullable: false),
                        Categories = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Contestant_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contestants", t => t.Contestant_ID)
                .Index(t => t.Contestant_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Media", "Contestant_ID", "dbo.Contestants");
            DropIndex("dbo.Media", new[] { "Contestant_ID" });
            DropTable("dbo.Media");
            DropTable("dbo.Contestants");
        }
    }
}
