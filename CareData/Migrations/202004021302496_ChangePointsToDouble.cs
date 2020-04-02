namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePointsToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Item", "PointCost", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Item", "PointCost", c => c.Int(nullable: false));
        }
    }
}
