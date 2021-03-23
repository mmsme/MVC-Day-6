namespace MVC_Day_6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class four : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentCourses", "Degree", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentCourses", "Degree");
        }
    }
}
