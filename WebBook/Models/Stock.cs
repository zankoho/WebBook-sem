using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace WebBook.Models
{
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Required]
        public int WarehauseId { get; set; }
        public Warehause Warehause { get; set; }
        [DisplayName("In stock")]
        public int Quantity { get; set; }
    }
}
