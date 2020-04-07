namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeader", "PullStartedName", c => c.String(maxLength: 128));
            CreateIndex("dbo.OrderHeader", "PullStartedName");
            AddForeignKey("dbo.OrderHeader", "PullStartedName", "dbo.ApplicationUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHeader", "PullStartedName", "dbo.ApplicationUser");
            DropIndex("dbo.OrderHeader", new[] { "PullStartedName" });
            DropColumn("dbo.OrderHeader", "PullStartedName");
        }
    }
}
