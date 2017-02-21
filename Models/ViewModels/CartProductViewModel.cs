namespace OnlineShop.Models.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CartProductViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be negative")]
        public int Quantity { get; set; }
        
        public decimal Price { get; set; }

        public byte[] Picture { get; set; }
    }
}