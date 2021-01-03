namespace Serenity_Craft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropBookRatingProperty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "BookRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "BookRating", c => c.Double(nullable: false));
        }
    }
}
