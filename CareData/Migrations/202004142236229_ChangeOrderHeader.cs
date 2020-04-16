namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOrderHeader : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OrderHeader", new[] { "PullStartedName" });
            DropColumn("dbo.OrderHeader", "PullStartedBy");
            RenameColumn(table: "dbo.OrderHeader", name: "PullStartedName", newName: "PullStartedBy");
            AlterColumn("dbo.OrderHeader", "PullStartedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.OrderHeader", "PullStartedBy");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrderHeader", new[] { "PullStartedBy" });
            AlterColumn("dbo.OrderHeader", "PullStartedBy", c => c.String());
            RenameColumn(table: "dbo.OrderHeader", name: "PullStartedBy", newName: "PullStartedName");
            AddColumn("dbo.OrderHeader", "PullStartedBy", c => c.String());
            CreateIndex("dbo.OrderHeader", "PullStartedName");
        }
    }
}
