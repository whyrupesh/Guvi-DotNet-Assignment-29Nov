using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagementApi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string OwnerId { get; set; } = string.Empty;
    }
}
