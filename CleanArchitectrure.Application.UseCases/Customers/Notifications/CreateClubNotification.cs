using MediatR;
using RunGroop.Data.Models.Data;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Notifications
{
    public record CreateClubNotification(Club createClubVM):INotification;
}
