namespace Serenity_Craft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReviewTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reviews", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reviews", "UserName", c => c.String(nullable: false));
        }
    }
}
