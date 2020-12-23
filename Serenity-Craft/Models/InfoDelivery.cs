namespace Serenity_Craft_Library.Models
{
    public class InfoDelivery
    {
        public int InfoDeliveryId { get; set; }
        public int DeliveryId { get; set; }
        public int BookId { get; set; }
        public int NoOfBooks { get; set; }

        // -- navigation properties --

        public virtual Delivery Delivery { get; set; }
        public virtual Book Book { get; set; }
    }
}