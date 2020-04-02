namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TryToFixUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BarCode", "AddedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.BarCode", "AddedBy");
            AddForeignKey("dbo.BarCode", "AddedBy", "dbo.ApplicationUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BarCode", "AddedBy", "dbo.ApplicationUser");
            DropIndex("dbo.BarCode", new[] { "AddedBy" });
            DropColumn("dbo.BarCode", "AddedBy");
        }
    }
}
