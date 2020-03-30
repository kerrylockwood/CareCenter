namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BarCode", "CreateBy", c => c.Guid(nullable: false));
            AddColumn("dbo.BarCode", "CreateAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.TimeSlot", "MaxPerSlot", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TimeSlot", "MaxPerSlot");
            DropColumn("dbo.BarCode", "CreateAt");
            DropColumn("dbo.BarCode", "CreateBy");
        }
    }
}
