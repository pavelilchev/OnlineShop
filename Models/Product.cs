namespace OnlineShop.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be negative")]
        public int Quantity { get; set; }

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "Price can't be negative")]
        public decimal Price { get; set; }

        [Required]
        public byte[] Picture { get; set; }
    }
}