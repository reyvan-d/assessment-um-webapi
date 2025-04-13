using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class GetAllUsersResponse
    {
        public GetAllUsersResponse()
        {
            Users = new List<UserGroupStruct>();
        }
        public List<UserGroupStruct> Users { get; set; }
    }

    public class UserGroupStruct
    {
        public UserGroupStruct(long userId, string userName, Dictionary<long, string> groups)
        {
            UserId = userId;
            UserName = userName;
            Groups = groups;
        }

        public long UserId { get; set; }
        public string UserName { get; set; }
        public Dictionary<long, string> Groups { get; set; }
    }
}
