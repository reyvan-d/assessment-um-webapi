namespace API.Models
{
    public class Permission
    {
        public long PermissionId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<GroupPermission> Groups { get; set; }
    }
}
