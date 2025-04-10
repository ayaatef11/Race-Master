using RunGroop.Data.Models.Entities;
using RunGroop.Data.Models.SignalR;
using RunGroopWebApp.Services.ViewModels;

namespace RunGroopWebApp.Services.interfaces
{
    public  interface INotificationService
    {
       Task<List<Notification> >GetAllNotifications(int nToUserId,bool getOnlyUnRead);
        Task<bool> SendNotificationToUserAsync(string userId, string message);
        NotificationPreferences GetUserNotificationPreferences(string userId);

        // Update the user's notification preferences
        void UpdateNotificationPreferences(string userId, NotificationPreferencesViewModel preferences);

    }
}
