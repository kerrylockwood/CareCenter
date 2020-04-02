namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChgAllTablesUserToString1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Customer", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Item", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.SubCategory", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.OrderHeader", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TimeSlot", "CreateBy", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Category", "CreateBy");
            CreateIndex("dbo.Customer", "CreateBy");
            CreateIndex("dbo.Item", "CreateBy");
            CreateIndex("dbo.SubCategory", "CreateBy");
            CreateIndex("dbo.OrderHeader", "CreateBy");
            CreateIndex("dbo.TimeSlot", "CreateBy");
            AddForeignKey("dbo.Category", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Customer", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: false);
            AddForeignKey("dbo.SubCategory", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Item", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: false);
            AddForeignKey("dbo.TimeSlot", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: false);
            AddForeignKey("dbo.OrderHeader", "CreateBy", "dbo.ApplicationUser", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderHeader", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.TimeSlot", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Item", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.SubCategory", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Customer", "CreateBy", "dbo.ApplicationUser");
            DropForeignKey("dbo.Category", "CreateBy", "dbo.ApplicationUser");
            DropIndex("dbo.TimeSlot", new[] { "CreateBy" });
            DropIndex("dbo.OrderHeader", new[] { "CreateBy" });
            DropIndex("dbo.SubCategory", new[] { "CreateBy" });
            DropIndex("dbo.Item", new[] { "CreateBy" });
            DropIndex("dbo.Customer", new[] { "CreateBy" });
            DropIndex("dbo.Category", new[] { "CreateBy" });
            AlterColumn("dbo.TimeSlot", "CreateBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.OrderHeader", "CreateBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.SubCategory", "CreateBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.Item", "CreateBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.Customer", "CreateBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.Category", "CreateBy", c => c.String(nullable: false));
        }
    }
}
