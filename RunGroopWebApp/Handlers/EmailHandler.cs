using MediatR;
using RunGroop.Data.Interfaces.Repositories;
using RunGroopWebApp.Notifications;

namespace RunGroopWebApp.Handlers
{
    public class EmailHandler(IClubRepository _clubRepository) : INotificationHandler<CreateClubNotification>
    {
        public async  Task Handle(CreateClubNotification notification, CancellationToken cancellationToken)
        {
            await _clubRepository.EventOccured(notification.createClubVM, "Email Sent");
            await Task.CompletedTask;
                }
    }
}
