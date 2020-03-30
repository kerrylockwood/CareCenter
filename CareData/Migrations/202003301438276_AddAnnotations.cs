namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer", "FirstName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Customer", "LastName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customer", "Address", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customer", "City", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Customer", "State", c => c.String(nullable: false, maxLength: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "State", c => c.String(nullable: false));
            AlterColumn("dbo.Customer", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Customer", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Customer", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.Customer", "FirstName", c => c.String(nullable: false));
        }
    }
}
