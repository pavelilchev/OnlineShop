namespace OnlineShop.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CartProduct
    {
        [Key]
        public int Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual Cart Cart { get; set; }

        public int Quantity { get; set; }
    }
}