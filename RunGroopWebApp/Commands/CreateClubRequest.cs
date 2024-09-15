using MediatR;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Commands
{
    public record CreateClubRequest(CreateClubViewModel cc) :IRequest;

}
