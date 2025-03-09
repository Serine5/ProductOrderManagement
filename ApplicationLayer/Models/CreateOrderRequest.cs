using System.ComponentModel.DataAnnotations;

namespace ProductOrderManagement.Models
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "At least one order item is required.")]
        public List<OrderItemRequest> OrderItems { get; set; }

        [Required(ErrorMessage = "User Id is required.")]
        public string UserId { get; set; }
    }
}