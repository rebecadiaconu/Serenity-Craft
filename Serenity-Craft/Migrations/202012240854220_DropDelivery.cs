namespace Serenity_Craft.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropDelivery : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InfoDeliveries", "BookId", "dbo.Books");
            DropForeignKey("dbo.InfoDeliveries", "DeliveryId", "dbo.Deliveries");
            DropForeignKey("dbo.Addresses", "DeliveryId", "dbo.Deliveries");
            DropIndex("dbo.Addresses", new[] { "DeliveryId" });
            DropIndex("dbo.InfoDeliveries", new[] { "DeliveryId" });
            DropIndex("dbo.InfoDeliveries", new[] { "BookId" });
            DropTable("dbo.Addresses");
            DropTable("dbo.Deliveries");
            DropTable("dbo.InfoDeliveries");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InfoDeliveries",
                c => new
                    {
                        InfoDeliveryId = c.Int(nullable: false, identity: true),
                        DeliveryId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        NoOfBooks = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InfoDeliveryId);
            
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        DeliveryId = c.Int(nullable: false, identity: true),
                        DeliveryDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(),
                        CardNumber = c.Int(nullable: false),
                        Total = c.Double(nullable: false),
                        PhoneNumber = c.String(),
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DeliveryId);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        DeliveryId = c.Int(nullable: false),
                        County = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        StreetNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DeliveryId);
            
            CreateIndex("dbo.InfoDeliveries", "BookId");
            CreateIndex("dbo.InfoDeliveries", "DeliveryId");
            CreateIndex("dbo.Addresses", "DeliveryId");
            AddForeignKey("dbo.Addresses", "DeliveryId", "dbo.Deliveries", "DeliveryId");
            AddForeignKey("dbo.InfoDeliveries", "DeliveryId", "dbo.Deliveries", "DeliveryId", cascadeDelete: true);
            AddForeignKey("dbo.InfoDeliveries", "BookId", "dbo.Books", "BookId", cascadeDelete: true);
        }
    }
}
