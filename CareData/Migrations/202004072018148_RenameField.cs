namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Item", "AisleNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Item", "IsleNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Item", "IsleNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Item", "AisleNumber");
        }
    }
}
