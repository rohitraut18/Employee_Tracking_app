namespace EmployeeTrackingWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelAddedTracking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trackings",
                c => new
                    {
                        TrackingId = c.Int(nullable: false, identity: true),
                        AppUserId = c.Int(nullable: false),
                        Lat = c.Double(nullable: false),
                        Long = c.Double(nullable: false),
                        OnDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TrackingId)
                .ForeignKey("dbo.AppUsers", t => t.AppUserId, cascadeDelete: true)
                .Index(t => t.AppUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trackings", "AppUserId", "dbo.AppUsers");
            DropIndex("dbo.Trackings", new[] { "AppUserId" });
            DropTable("dbo.Trackings");
        }
    }
}
