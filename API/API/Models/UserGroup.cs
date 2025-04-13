using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserGroup
    {
        public long UserId { get; set; }
        public long GroupId { get; set; }
        public virtual User User { get; set; }
        public virtual Group Group { get; set; }

        [Key]
        public int Id => throw new NotImplementedException();
    }
}
