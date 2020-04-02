namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChgGuidToString2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser");
            DropIndex("dbo.BarCode", new[] { "CreateBy" });
            AlterColumn("dbo.BarCode", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Category", "CreateBy", c => c.String(nullable: false));
            CreateIndex("dbo.BarCode", "CreateBy");
            AddForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser");
            DropIndex("dbo.BarCode", new[] { "CreateBy" });
            AlterColumn("dbo.Category", "CreateBy", c => c.String());
            AlterColumn("dbo.BarCode", "CreateBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.BarCode", "CreateBy");
            AddForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser", "Id");
        }
    }
}
