using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class AddGroupRequest
    {
        [Required]
        public string Name { get; set; }
        public List<long> PermissionIds { get; set; }
    }
}
