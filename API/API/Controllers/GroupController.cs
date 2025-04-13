using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<UserController> _logger;

        public GroupController(DataContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetGroupCount")]
        public IActionResult GetGroupCount()
        {
            try
            {
                var result = (from Group in _context.Groups select Group.GroupId).LongCount();
                return Ok(new { Result = result });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("GetAllGroups")]
        public IActionResult GetAllGroups()
        {
            try
            {
                var query = from Group in _context.Groups select new { Group.GroupId, Group.Name };
                Dictionary<long, string> groups = new Dictionary<long, string>();

                foreach (var group in query)
                    groups.Add(group.GroupId, group.Name);

                return Ok(new { Groups = groups });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("GetTotalUsersForGroup")]
        public IActionResult GetTotalUsersForGroup([FromBody] DTO.GetTotalUsersForGroupRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = (from g in _context.Groups
                            join ug in _context.UserGroups on g.GroupId equals ug.GroupId
                            where g.GroupId == request.GroupId
                            select ug.GroupId).LongCount();
                return Ok(new { Result = result});
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup([FromBody] DTO.AddGroupRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var group = new Models.Group { Name = request.Name };
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();

                foreach (var id in request.PermissionIds)
                    _context.GroupPermissions.Add(new Models.GroupPermission { GroupId = group.GroupId, PermissionId = id });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
