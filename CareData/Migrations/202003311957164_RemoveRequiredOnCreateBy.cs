namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredOnCreateBy : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Category", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Customer", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Item", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.OrderHeader", "CreateBy", "dbo.ApplicationUser");
            DropIndex("dbo.BarCode", new[] { "CreateBy" });
            DropIndex("dbo.Category", new[] { "CreateBy" });
            DropIndex("dbo.Customer", new[] { "CreateBy" });
            DropIndex("dbo.Item", new[] { "CreateBy" });
            DropIndex("dbo.OrderHeader", new[] { "CreateBy" });
            AlterColumn("dbo.BarCode", "CreateBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Category", "CreateBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Customer", "CreateBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Item", "CreateBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.OrderHeader", "CreateBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.BarCode", "CreateBy");
            CreateIndex("dbo.Category", "CreateBy");
            CreateIndex("dbo.Customer", "CreateBy");
            CreateIndex("dbo.Item", "CreateBy");
            CreateIndex("dbo.OrderHeader", "CreateBy");
            AddForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser", "Id");
            AddForeignKey("dbo.Category", "CreateBy", "dbo.ApplicationUser", "Id");
            AddForeignKey("dbo.Customer", "CreateBy", "dbo.ApplicationUser", "Id");
            AddForeignKey("dbo.Item", "CreateBy", "dbo.ApplicationUser", "Id");
            AddForeignKey("dbo.OrderHeader", "CreateBy", "dbo.ApplicationUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHeader", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Item", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Customer", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Category", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser");
            DropIndex("dbo.OrderHeader", new[] { "CreateBy" });
            DropIndex("dbo.Item", new[] { "CreateBy" });
            DropIndex("dbo.Customer", new[] { "CreateBy" });
            DropIndex("dbo.Category", new[] { "CreateBy" });
            DropIndex("dbo.BarCode", new[] { "CreateBy" });
            AlterColumn("dbo.OrderHeader", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Item", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Customer", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Category", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.BarCode", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.OrderHeader", "CreateBy");
            CreateIndex("dbo.Item", "CreateBy");
            CreateIndex("dbo.Customer", "CreateBy");
            CreateIndex("dbo.Category", "CreateBy");
            CreateIndex("dbo.BarCode", "CreateBy");
            AddForeignKey("dbo.OrderHeader", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Item", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Customer", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Category", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BarCode", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: true);
        }
    }
}
