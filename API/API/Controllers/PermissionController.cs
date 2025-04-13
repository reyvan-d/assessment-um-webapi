using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ILogger<UserController> _logger;

        public PermissionController(DataContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetPermissionCount")]
        public IActionResult GetPermissionCount()
        {
            try
            {
                var result = (from Permission in _context.Permissions select Permission.PermissionId).LongCount();
                return Ok(new { Result = result });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }

        [HttpPost("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] DTO.AddPermissionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                var permission = new Models.Permission { Name = request.Name };
                _context.Permissions.Add(permission);
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
