namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePullUserDefinition : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderHeader", "PullStartedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderHeader", "PullStartedBy", c => c.Guid());
        }
    }
}
