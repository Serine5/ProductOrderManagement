using System.ComponentModel.DataAnnotations;

namespace ProductOrderManagement.Models
{
    public class OrderItemRequest
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
