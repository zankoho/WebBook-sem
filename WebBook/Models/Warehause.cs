using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace WebBook.Models
{
    public class Warehause
    {
        [Key]
        public int WarehauseId { get; set; }
        [Required]
        public string WarehauseLocation { get; set;}
        public string? WarehauseName { get; set; } = "";

    }
}
