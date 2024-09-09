
using RunGroop.Data.Models;

namespace RunGroopWebApp.Services.interfaces
{
    public  interface INotificationService
    {
       Task< List<Notification> >GetAllNotifications(int nToUserId,bool getOnlyUnRead);
    }
}
