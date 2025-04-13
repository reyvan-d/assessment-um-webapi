using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class DeleteUserRequest
    {
        [Required]
        public long UserId { get; set; }
    }
}
