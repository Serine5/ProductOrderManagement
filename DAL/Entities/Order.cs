namespace DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        // if want to enable lazy loading, must declare navigation properties as virtual
        public ICollection<OrderItem> OrderItems { get; set; } 
    }
}
