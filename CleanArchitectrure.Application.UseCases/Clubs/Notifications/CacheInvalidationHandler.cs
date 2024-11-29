using MediatR;
using RunGroop.Repository.Interfaces;
using RunGroop.UseCases.Clubs.Notifications;

namespace RunGroop.UseCases.Customers.Notifications
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
