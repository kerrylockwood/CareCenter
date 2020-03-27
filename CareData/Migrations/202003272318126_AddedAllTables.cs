namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAllTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        SubCatId = c.Int(nullable: false),
                        ItemName = c.String(nullable: false),
                        IsleNumber = c.Int(nullable: false),
                        MaxAllowed = c.Int(nullable: false),
                        PointCost = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId)
                .ForeignKey("dbo.SubCategory", t => t.SubCatId, cascadeDelete: true)
                .Index(t => t.SubCatId);
            
            CreateTable(
                "dbo.SubCategory",
                c => new
                    {
                        SubCatId = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        SubCatName = c.String(nullable: false),
                        SubCatMaxAllowed = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SubCatId)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        OrderDetailId = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Filled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Item", t => t.ItemId, cascadeDelete: true)
                .ForeignKey("dbo.OrderHeader", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.OrderHeader",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        CustId = c.Int(nullable: false),
                        SlotId = c.Int(nullable: false),
                        MostWantedNotes = c.String(nullable: false, maxLength: 1000),
                        FreezerNotes = c.String(nullable: false, maxLength: 1000),
                        ProduceNotes = c.String(nullable: false, maxLength: 1000),
                        NonFoodNotes = c.String(nullable: false, maxLength: 1000),
                        Deliver = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        PullStartedAt = c.DateTime(),
                        OrderCompletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Customer", t => t.CustId, cascadeDelete: true)
                .ForeignKey("dbo.TimeSlot", t => t.SlotId, cascadeDelete: true)
                .Index(t => t.CustId)
                .Index(t => t.SlotId);
            
            CreateTable(
                "dbo.TimeSlot",
                c => new
                    {
                        SlotId = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        Time = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.SlotId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.OrderHeader");
            DropForeignKey("dbo.OrderHeader", "SlotId", "dbo.TimeSlot");
            DropForeignKey("dbo.OrderHeader", "CustId", "dbo.Customer");
            DropForeignKey("dbo.OrderDetail", "ItemId", "dbo.Item");
            DropForeignKey("dbo.Item", "SubCatId", "dbo.SubCategory");
            DropForeignKey("dbo.SubCategory", "CategoryId", "dbo.Category");
            DropIndex("dbo.OrderHeader", new[] { "SlotId" });
            DropIndex("dbo.OrderHeader", new[] { "CustId" });
            DropIndex("dbo.OrderDetail", new[] { "ItemId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.SubCategory", new[] { "CategoryId" });
            DropIndex("dbo.Item", new[] { "SubCatId" });
            DropTable("dbo.TimeSlot");
            DropTable("dbo.OrderHeader");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.SubCategory");
            DropTable("dbo.Item");
            DropTable("dbo.Category");
        }
    }
}
