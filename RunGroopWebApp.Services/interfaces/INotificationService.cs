using RunGroop.Data.Models.SignalR;

namespace RunGroopWebApp.Services.interfaces
{
    public  interface INotificationService
    {
       Task< List<Notification> >GetAllNotifications(int nToUserId,bool getOnlyUnRead);
    }
}
