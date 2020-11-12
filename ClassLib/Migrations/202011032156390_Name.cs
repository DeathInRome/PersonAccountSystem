namespace ClassLib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Name : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Educations", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Positions", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.Educations", new[] { "EmployeeId" });
            DropIndex("dbo.Positions", new[] { "EmployeeId" });
            AddColumn("dbo.Employees", "Education", c => c.String());
            AddColumn("dbo.Employees", "Position", c => c.String());
            DropTable("dbo.Educations");
            DropTable("dbo.Positions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PositionId);
            
            CreateTable(
                "dbo.Educations",
                c => new
                    {
                        EducationId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EducationId);
            
            DropColumn("dbo.Employees", "Position");
            DropColumn("dbo.Employees", "Education");
            CreateIndex("dbo.Positions", "EmployeeId");
            CreateIndex("dbo.Educations", "EmployeeId");
            AddForeignKey("dbo.Positions", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
            AddForeignKey("dbo.Educations", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
    }
}
