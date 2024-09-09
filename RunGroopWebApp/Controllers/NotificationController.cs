
using RunGroopWebApp.Services.interfaces;

namespace RunGroopWebApp.Controllers
{
    public class NotificationController(INotificationService _NotificationService) : Controller
    {
     
        public IActionResult AllNotifications()
        {

            return View();

        }
        public JsonResult GetNotifications(int ToUserId,bool bIsGetOnlyUnread = false)//optional paramter must appear after the reruired parameter means that the assignment goes from right to left 
        {
          
            return Json(_NotificationService.GetAllNotifications(ToUserId, bIsGetOnlyUnread));

        }
    } }

