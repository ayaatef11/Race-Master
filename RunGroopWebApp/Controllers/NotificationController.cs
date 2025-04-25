
using GraphQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RunGroop.Data.Data;
using RunGroop.Infrastructure;
using RunGroopWebApp.Services.Services;
using RunGroopWebApp.Services.Services.interfaces;
using RunGroopWebApp.Services.ViewModels;

namespace RunGroopWebApp.Controllers
{
    [Authorize]
    public class NotificationController(INotificationService _NotificationService, ApplicationDbContext _context, SignalServer _hubContext) : Controller
    {
     
        public IActionResult AllNotifications()
        {

            return View();

        }
        public async Task<IActionResult> SendNotificationToUser(string userId, string message)
        {
            var result = await _NotificationService.SendNotificationToUserAsync(userId, message);

            if (result)
            {
                return Json(new { success = true, message = "Notification sent successfully." });
            }

            return Json(new { success = false, message = "Failed to send notification." });
        }
        public JsonResult GetNotifications(int ToUserId,bool bIsGetOnlyUnread = false)//optional paramter must appear after the reruired parameter means that the assignment goes from right to left 
        {
          
            return Json(_NotificationService.GetAllNotifications(ToUserId, bIsGetOnlyUnread));

        }
        [HttpGet]
        public async Task<IActionResult> ReadNotification(int notificationId)
        {
            try
            {
                var userId = "019a2c4a-9741-4552-a45b-7926d24882dc";//User.Identity.GetUserId();

                var notification = await _context.Notifications
                    //.Where(n => n.Id == notificationId && n.UserId == userId)
                    .FirstOrDefaultAsync();

                if (notification == null)
                {
                    return Json(new { success = false, message = "Notification not found" });
                }

                // Mark as read
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Get unread count for the user to return
                var unreadCount = await _context.Notifications
                    //.Where(n => n.UserId == userId && !n.IsRead)
                    .CountAsync();

                // Notify clients of the update through SignalR
                await _hubContext.Clients.User(userId).SendAsync("displayNotification");

                return Json(new
                {
                    success = true,
                    message = "Notification marked as read",
                    unreadCount = unreadCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
         [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                // Get current user ID
                //var userId = User.Identity.GetUserId();
                var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
                var notifications = await _context.Notifications
                    //.Where(n => n.UserId == userId && n.CreatedAt >= thirtyDaysAgo)
                    .OrderByDescending(n => n.CreatedDate)
                    .Take(50)
                    .Select(n => new {
                        n.Id,
                        n.Message,
                        n.IsRead,
                        n.CreatedDate,
                        n.NotificationType
                    })
                    .ToListAsync();

                var unreadCount = await _context.Notifications
                    //.Where(n => n.UserId == userId && !n.IsRead)
                    .CountAsync();

                return Json(new
                {
                    success = true,
                    notifications = notifications,
                    unreadCount = unreadCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        public IActionResult Preferences()
        {
            // Assuming the user's current preferences are fetched from the database
            var preferences =  _NotificationService.GetUserNotificationPreferences(User.Identity.Name);

            var viewModel = new NotificationPreferencesViewModel
            {
                NotifyMention = preferences.NotifyMention,
                NotifyRequest = preferences.NotifyRequest,
                NotifyShare = preferences.NotifyShare,
                NotifyMessage = preferences.NotifyMessage,
                NotifyAdds = preferences.NotifyAdds,
                NotifySales = preferences.NotifySales
            };

            return View(viewModel);
        }

        // POST: Handle the form submission
        [HttpPost]
        public IActionResult UpdatePreferences(NotificationPreferencesViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Call the service to update preferences in the database
                _NotificationService.UpdateNotificationPreferences(User.Identity.Name, model);

                // Optionally, you could return a success message or redirect
                TempData["Success"] = "Your preferences have been updated successfully!";
                return RedirectToAction("Preferences");
            }

            // If the model is invalid, show the form again with the current data
            return View("Preferences", model);
        }
    }
} 


