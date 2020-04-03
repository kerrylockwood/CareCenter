namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModCustomerTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customer", "CreatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customer", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
    }
}
