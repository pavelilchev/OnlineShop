namespace OnlineShop.Models.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using Attributes;

    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Plese enter product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Plese enter product Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity can't be negative")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Plese enter product Price")]
        [Range(0, long.MaxValue, ErrorMessage = "Price can't be negative")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Plese chose a Image")]
        [FileType("JPG,JPEG,PNG")]
        public HttpPostedFileBase Picture { get; set; }
    }
}