using MediatR;
using RunGroop.Data.Models.Data;

namespace RunGroop.UseCases.Clubs.Notifications
{
    public record CreateClubNotification(Club createClubVM) : INotification;
}
