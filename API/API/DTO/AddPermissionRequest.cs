using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class AddPermissionRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
