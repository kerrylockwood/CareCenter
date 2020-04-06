namespace CareData.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChgTimeSlot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeSlot", "DayOfWeekNum", c => c.Int(nullable: false));
            DropColumn("dbo.TimeSlot", "DayOfWeek");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TimeSlot", "DayOfWeek", c => c.Int(nullable: false));
            DropColumn("dbo.TimeSlot", "DayOfWeekNum");
        }
    }
}
