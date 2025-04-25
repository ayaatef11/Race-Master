using Microsoft.AspNetCore.SignalR;
using RunGroop.Data.Data;
using RunGroop.Data.Models.Entities;
using RunGroop.Data.Models.SignalR;
using RunGroop.Infrastructure;
using RunGroopWebApp.Services.Services.interfaces;
using RunGroopWebApp.Services.ViewModels;

namespace RunGroopWebApp.Services.Services
{
    public class NotificationService(ApplicationDbContext _context, IHubContext<SignalServer> _hubContext) : INotificationService
    {
        public async Task<List<Notification>> GetAllNotifications(int nToUserId, bool bIsGetOnlyUnread)
        {
            var notificationsQuery = _context.Notifications.AsQueryable();
            //notificationsQuery = notificationsQuery.Where(n => n.ToUserId == nToUserId);
            if (bIsGetOnlyUnread)
            {
                notificationsQuery = notificationsQuery.Where(n => !n.IsRead);
            }

            return await notificationsQuery.ToListAsync();
        }
        public async Task<bool> SendNotificationToUserAsync(string userId, string message)
        {
            try
            {
                var notification = new Notification
                {
                    //UserId = userId,
                    Message = message,
                    IsRead = false,
                    CreatedDate = DateTime.UtcNow
                };

                await _context.Notifications.AddAsync(notification);
                await _context.SaveChangesAsync();

                // Trigger real-time notification (SignalR)
                await _hubContext.Clients.User(userId).SendAsync("displayNotification");

                return true;
            }
            catch (Exception ex)
            {
                // Log error if needed
                return false;
            }
        }
        public NotificationPreferences GetUserNotificationPreferences(string userId)
        {
            // Fetch from the database (assumes there’s a table for preferences linked to the user)
            return _context.NotificationPreferences
                           .FirstOrDefault(np => np.UserId == userId);
        }

        // Update the notification preferences for the current user
        public void UpdateNotificationPreferences(string userId, NotificationPreferencesViewModel preferences)
        {
            var existingPreferences = _context.NotificationPreferences
                                               .FirstOrDefault(np => np.UserId == userId);

            if (existingPreferences != null)
            {
                // Update the preferences
                existingPreferences.NotifyMention = preferences.NotifyMention;
                existingPreferences.NotifyRequest = preferences.NotifyRequest;
                existingPreferences.NotifyShare = preferences.NotifyShare;
                existingPreferences.NotifyMessage = preferences.NotifyMessage;
                existingPreferences.NotifyAdds = preferences.NotifyAdds;
                existingPreferences.NotifySales = preferences.NotifySales;

                _context.SaveChanges();
            }
            else
            {
                // If no existing preferences, create a new entry
                var newPreferences = new NotificationPreferences
                {
                    UserId = userId,
                    NotifyMention = preferences.NotifyMention,
                    NotifyRequest = preferences.NotifyRequest,
                    NotifyShare = preferences.NotifyShare,
                    NotifyMessage = preferences.NotifyMessage,
                    NotifyAdds = preferences.NotifyAdds,
                    NotifySales = preferences.NotifySales
                };

                _context.NotificationPreferences.Add(newPreferences);
                _context.SaveChanges();
            }

        }

       
    }
    }


