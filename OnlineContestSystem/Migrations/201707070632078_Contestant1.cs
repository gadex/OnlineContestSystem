namespace OnlineContestSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Contestant1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Voters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoreyId = c.Int(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Contestants", "VoteCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contestants", "VoteCount");
            DropTable("dbo.Voters");
        }
    }
}
