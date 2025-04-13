using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class GetTotalUsersForGroupRequest
    {
        [Required]
        public long GroupId { get; set; }
    }
}
