namespace EmployeeTrackingWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsAddedAuthentications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppRoles",
                c => new
                    {
                        AppRoleId = c.Int(nullable: false, identity: true),
                        AppRoleName = c.String(),
                    })
                .PrimaryKey(t => t.AppRoleId);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        AppUserId = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        MobileNo = c.String(nullable: false, maxLength: 10),
                        EmailId = c.String(nullable: false, maxLength: 256),
                        Password = c.String(nullable: false, maxLength: 30),
                        AppRoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppUserId)
                .ForeignKey("dbo.AppRoles", t => t.AppRoleId, cascadeDelete: true)
                .Index(t => t.AppRoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUsers", "AppRoleId", "dbo.AppRoles");
            DropIndex("dbo.AppUsers", new[] { "AppRoleId" });
            DropTable("dbo.AppUsers");
            DropTable("dbo.AppRoles");
        }
    }
}
