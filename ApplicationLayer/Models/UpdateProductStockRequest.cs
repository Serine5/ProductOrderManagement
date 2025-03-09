using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.Models
{
    public class UpdateProductStockRequest
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number.")]
        public int NewQuantity { get; set; }
    }
}