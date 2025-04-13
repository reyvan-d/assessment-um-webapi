using API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(DataContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetUserCount")]
        public IActionResult GetUserCount()
        {
            try
            {
                var result = (from user in _context.Users select user.UserId).LongCount(); ;
                return Ok(new { Result = result });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var query = from u in _context.Users
                        join ug in _context.UserGroups on u.UserId equals ug.UserId
                        join g in _context.Groups on ug.GroupId equals g.GroupId
                        select new
                        {
                            UserId = u.UserId,
                            UserName = u.Name,
                            GroupId = g.GroupId,
                            GroupName = g.Name
                        };

            var resp = new GetAllUsersResponse();
            foreach (var item in query)
            {
                if (!resp.Users.Exists(ud => ud.UserId == item.UserId))
                    resp.Users.Add(new UserGroupStruct(item.UserId, item.UserName, new Dictionary<long, string>() { { item.GroupId, item.GroupName } }));
                else
                    resp.Users.FirstOrDefault(ud => ud.UserId == item.UserId).Groups.Add(item.GroupId, item.GroupName);
            }
            return Ok(new { Result = resp });
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] DTO.AddUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var user = new Models.User { Name = request.Name };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                foreach (var id in request.GroupIds)
                    _context.UserGroups.Add(new Models.UserGroup { UserId = user.UserId, GroupId = id });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Result = $"User Added [{user.UserId}]" });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] DTO.GetUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var User = _context.Users.SingleOrDefault(u => u.UserId == request.UserId);

                if (null == User)
                    return BadRequest(new { Error = "Invalid UserId" });

                return Ok(new {UserId = User.UserId, UserName = User.Name});
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] DTO.UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                var User = _context.Users.SingleOrDefault(u => u.UserId == request.UserId);

                if (null == User)
                    return BadRequest(new { Error = "Invalid UserId" });

                if (!String.IsNullOrEmpty(request.Name))
                    User.Name = request.Name;

                var query = from u in _context.Users
                            join ug in _context.UserGroups on u.UserId equals ug.UserId
                            where u.UserId == request.UserId
                            select ug.GroupId;

                HashSet<long> current = [.. query];
                HashSet<long> updated = [.. request.GroupIds];

                var idsToDelete = current.Except(updated);
                var idsToInsert = updated.Except(current);

                foreach (var id in idsToDelete)
                {
                    var UserGroup = new Models.UserGroup { UserId = request.UserId, GroupId = id };
                    _context.UserGroups.Remove(UserGroup);
                }

                foreach (var id in idsToInsert)
                    _context.UserGroups.Add(new Models.UserGroup { UserId = request.UserId, GroupId = id });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }

            return Ok(new { Result = "User Updated" });
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] DTO.DeleteUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                var User = _context.Users.SingleOrDefault(u => u.UserId == request.UserId);

                if (null == User)
                    return BadRequest(new { Error = "Invalid UserId" });

                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }

            return Ok(new { Result = "User Deleted" });
        }
    }
}
