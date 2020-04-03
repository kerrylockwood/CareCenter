namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromOrderHeader : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderHeader", "MostWantedNotes", c => c.String(maxLength: 1000));
            AlterColumn("dbo.OrderHeader", "FreezerNotes", c => c.String(maxLength: 1000));
            AlterColumn("dbo.OrderHeader", "ProduceNotes", c => c.String(maxLength: 1000));
            AlterColumn("dbo.OrderHeader", "NonFoodNotes", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderHeader", "NonFoodNotes", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.OrderHeader", "ProduceNotes", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.OrderHeader", "FreezerNotes", c => c.String(nullable: false, maxLength: 1000));
            AlterColumn("dbo.OrderHeader", "MostWantedNotes", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
