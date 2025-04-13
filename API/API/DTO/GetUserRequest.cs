using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class GetUserRequest
    {
        [Required]
        public long UserId { get; set; }
    }
}
