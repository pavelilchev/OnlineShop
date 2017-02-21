namespace OnlineShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Cart
    {
        public Cart()
        {
            this.Products = new HashSet<CartProduct>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public virtual ICollection<CartProduct> Products { get; set; }
    }
}