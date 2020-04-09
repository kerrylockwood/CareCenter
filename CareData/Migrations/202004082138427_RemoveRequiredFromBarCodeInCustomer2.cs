namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromBarCodeInCustomer2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customer", "BarCodeId", "dbo.BarCode");
            DropIndex("dbo.Customer", new[] { "BarCodeId" });
            AlterColumn("dbo.Customer", "BarCodeId", c => c.Int());
            CreateIndex("dbo.Customer", "BarCodeId");
            AddForeignKey("dbo.Customer", "BarCodeId", "dbo.BarCode", "BarCodeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "BarCodeId", "dbo.BarCode");
            DropIndex("dbo.Customer", new[] { "BarCodeId" });
            AlterColumn("dbo.Customer", "BarCodeId", c => c.Int(nullable: true));
            CreateIndex("dbo.Customer", "BarCodeId");
            AddForeignKey("dbo.Customer", "BarCodeId", "dbo.BarCode", "BarCodeId", cascadeDelete: false);
        }
    }
}
