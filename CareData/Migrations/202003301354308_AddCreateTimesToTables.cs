namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreateTimesToTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "CreateBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Category", "CreateAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Customer", "CreateBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Customer", "CreateAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Item", "CreateBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Item", "CreateAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.SubCategory", "CreateBy", c => c.Guid(nullable: false));
            AddColumn("dbo.SubCategory", "CreateAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.OrderHeader", "CreateBy", c => c.Guid(nullable: false));
            AddColumn("dbo.OrderHeader", "PullStartedBy", c => c.Guid());
            AddColumn("dbo.TimeSlot", "CreateBy", c => c.Guid(nullable: false));
            AddColumn("dbo.TimeSlot", "CreateAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.OrderHeader", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.OrderHeader", "PullStartedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.OrderHeader", "OrderCompletedAt", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderHeader", "OrderCompletedAt", c => c.DateTime());
            AlterColumn("dbo.OrderHeader", "PullStartedAt", c => c.DateTime());
            AlterColumn("dbo.OrderHeader", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.TimeSlot", "CreateAt");
            DropColumn("dbo.TimeSlot", "CreateBy");
            DropColumn("dbo.OrderHeader", "PullStartedBy");
            DropColumn("dbo.OrderHeader", "CreateBy");
            DropColumn("dbo.SubCategory", "CreateAt");
            DropColumn("dbo.SubCategory", "CreateBy");
            DropColumn("dbo.Item", "CreateAt");
            DropColumn("dbo.Item", "CreateBy");
            DropColumn("dbo.Customer", "CreateAt");
            DropColumn("dbo.Customer", "CreateBy");
            DropColumn("dbo.Category", "CreateAt");
            DropColumn("dbo.Category", "CreateBy");
        }
    }
}
