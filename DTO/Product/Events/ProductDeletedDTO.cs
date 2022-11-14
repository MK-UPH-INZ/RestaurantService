namespace RestaurantService.DTO.Product.Events
{
    public class ProductDeletedDTO
    {
        public int Id { get; set; }
        public string Event { get; set; } = "Product_Deleted";
    }
}
