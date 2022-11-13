namespace RestaurantService.DTO
{
    public class RestaurantDeletedDTO
    {
        public int Id { get; set; }

        public string Event { get; set; } = "Restaurant_Deleted";
    }
}
