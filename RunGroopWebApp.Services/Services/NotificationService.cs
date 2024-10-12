using RunGroop.Data.Data;
using RunGroop.Data.Models.SignalR;
using RunGroopWebApp.Services.interfaces;

namespace RunGroopWebApp.Services.Services
{
    public  class NotificationService(ApplicationDbContext _context):INotificationService
    {
        public async Task<List<Notification>> GetAllNotifications(int nToUserId, bool bIsGetOnlyUnread)
        {
            var notificationsQuery = _context.Notifications.AsQueryable();
            notificationsQuery = notificationsQuery.Where(n => n.ToUserId == nToUserId);
            if (bIsGetOnlyUnread)
            {
                notificationsQuery = notificationsQuery.Where(n => !n.IsRead);
            }

            return await notificationsQuery.ToListAsync();
        }

    }
    }

