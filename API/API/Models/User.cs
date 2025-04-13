namespace API.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public ICollection<UserGroup> Groups { get; set; }
    }
}
