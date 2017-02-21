namespace OnlineShop.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CartProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public virtual Cart Cart { get; set; }

        public int Quantity { get; set; }
    }
}