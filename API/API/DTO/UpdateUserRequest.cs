using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class UpdateUserRequest : AddUserRequest
    {
        public UpdateUserRequest(long userId, string name, List<long> groupIds) : base(name, groupIds)
        {
            UserId = userId;
        }

        [Required]
        public long UserId { get; set; }
    }
}
