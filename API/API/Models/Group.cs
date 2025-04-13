namespace API.Models
{
    public class Group
    {
        public long GroupId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserGroup> Users { get; set; }
        public virtual ICollection<GroupPermission> Permissions { get; set; }
    }
}
