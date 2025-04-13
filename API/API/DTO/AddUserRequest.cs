using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.DTO
{
    public class AddUserRequest
    {
        public AddUserRequest(string name, List<long> groupIds)
        {
            Name = name;
            GroupIds = groupIds;
        }

        [Required]
        public string Name { get; set; }
        public List<long> GroupIds { get; set; }
    }
}
