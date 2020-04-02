namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChgGuidToString1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BarCode", new[] { "AddedBy" });
            DropColumn("dbo.BarCode", "CreateBy");
            RenameColumn(table: "dbo.BarCode", name: "AddedBy", newName: "CreateBy");
            AlterColumn("dbo.BarCode", "CreateBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Category", "CreateBy", c => c.String());
            CreateIndex("dbo.BarCode", "CreateBy");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BarCode", new[] { "CreateBy" });
            AlterColumn("dbo.Category", "CreateBy", c => c.Guid(nullable: false));
            AlterColumn("dbo.BarCode", "CreateBy", c => c.Guid(nullable: false));
            RenameColumn(table: "dbo.BarCode", name: "CreateBy", newName: "AddedBy");
            AddColumn("dbo.BarCode", "CreateBy", c => c.Guid(nullable: false));
            CreateIndex("dbo.BarCode", "AddedBy");
        }
    }
}
