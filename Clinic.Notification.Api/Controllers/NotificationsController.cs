using Clinic.Notification.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Notification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationDbContext _context;

        public NotificationsController(NotificationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var list = await _context.Notifications.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return Ok(list);
        }
    }
}