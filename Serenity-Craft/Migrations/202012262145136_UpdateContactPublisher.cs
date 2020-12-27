namespace Serenity_Craft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateContactPublisher : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Contacts", name: "ContactId", newName: "PublisherId");
            RenameIndex(table: "dbo.Contacts", name: "IX_ContactId", newName: "IX_PublisherId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Contacts", name: "IX_PublisherId", newName: "IX_ContactId");
            RenameColumn(table: "dbo.Contacts", name: "PublisherId", newName: "ContactId");
        }
    }
}
