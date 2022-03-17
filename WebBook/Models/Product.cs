using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace WebBook.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [DisplayName("Product name")]
        [Required]
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
