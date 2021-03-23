namespace MVC_Day_6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class two : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartmentCourses",
                c => new
                    {
                        DeptID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DeptID, t.CourseID })
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DeptID, cascadeDelete: true)
                .Index(t => t.DeptID)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepartmentCourses", "DeptID", "dbo.Departments");
            DropForeignKey("dbo.DepartmentCourses", "CourseID", "dbo.Courses");
            DropIndex("dbo.DepartmentCourses", new[] { "CourseID" });
            DropIndex("dbo.DepartmentCourses", new[] { "DeptID" });
            DropTable("dbo.DepartmentCourses");
        }
    }
}
