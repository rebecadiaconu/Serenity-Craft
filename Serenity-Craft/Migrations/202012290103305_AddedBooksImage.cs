namespace Serenity_Craft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBooksImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ImagePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "ImagePath");
        }
    }
}
