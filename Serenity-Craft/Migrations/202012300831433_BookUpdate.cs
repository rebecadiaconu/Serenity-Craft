namespace Serenity_Craft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Books", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "ImagePath", c => c.String(nullable: false));
        }
    }
}
