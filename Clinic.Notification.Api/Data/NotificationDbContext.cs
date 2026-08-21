using Clinic.Notification.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Notification.Api.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) 
        {
        }
        public DbSet<NotificationLog> Notifications => Set<NotificationLog>();
    }
}
