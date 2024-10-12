using MediatR;
using RunGroop.Data.Interfaces.Repositories;
using RunGroopWebApp.Notifications;

namespace RunGroopWebApp.Handlers
{
    public class CacheInvalidationHandler(IClubRepository _clubRepository) : INotificationHandler<CreateClubNotification>
    {
        public async Task Handle(CreateClubNotification notification, CancellationToken cancellationToken)
        {
            await _clubRepository.EventOccured(notification.createClubVM, "Club Validated");
            await Task.CompletedTask;
        }
    }
}
