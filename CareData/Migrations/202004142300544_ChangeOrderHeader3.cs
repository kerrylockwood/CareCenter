namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeOrderHeader3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderHeader", "PullStartedName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderHeader", "PullStartedName");
        }
    }
}
