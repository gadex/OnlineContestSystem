namespace OnlineContestSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categorey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Contestants", "Category_Id", c => c.Int());
            CreateIndex("dbo.Contestants", "Category_Id");
            AddForeignKey("dbo.Contestants", "Category_Id", "dbo.Categories", "Id");
            DropColumn("dbo.Contestants", "Categories");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contestants", "Categories", c => c.Int(nullable: false));
            DropForeignKey("dbo.Contestants", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Contestants", new[] { "Category_Id" });
            DropColumn("dbo.Contestants", "Category_Id");
            DropTable("dbo.Categories");
        }
    }
}
