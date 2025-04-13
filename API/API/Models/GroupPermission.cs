using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class GroupPermission
    {
        public long GroupId { get; set; }
        public long PermissionId { get; set; }
        public virtual Group Group { get; set; }
        public virtual Permission Permission { get; set; }

        [Key]
        public int Id => throw new NotImplementedException();
    }
}
