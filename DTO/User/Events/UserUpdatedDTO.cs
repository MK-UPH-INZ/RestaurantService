namespace RestaurantService.DTO.User.Events
{
    public class UserUpdatedDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Event { get; set; }
    }
}
