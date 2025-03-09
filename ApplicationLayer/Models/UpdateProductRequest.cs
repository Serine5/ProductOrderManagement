using System.ComponentModel.DataAnnotations;

namespace ProductOrderManagement.Models
{
    public class UpdateProductRequest : CreateProductRequest
    {
        [Required(ErrorMessage = "Product Id is required.")]
        public int Id { get; set; }
    }
}