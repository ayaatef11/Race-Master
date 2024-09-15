using MediatR;
using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;
using RunGroopWebApp.Commands;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Handlers
{
    public class CreateClubHandler (IClubRepository clubRepository, IPhotoService _photoService) : IRequestHandler<CreateClubRequest>
    {

        public async Task  Handle(CreateClubRequest request, CancellationToken cancellationToken)
        {
            var result = await _photoService.AddPhotoAsync(request.cc.Image);
            var club = new Club
            {
                Title = request.cc.Title,
                Description = request.cc.Description,
                Image = result.Url.ToString(),
                ClubCategory = request.cc.ClubCategory,
                AppUserId = request.cc.AppUserId,
                Address = new Address
                {
                    Street = request.cc.Address.Street,
                    City = request.cc.Address.City,
                    State = request.cc.Address.State,
                }
            };
            clubRepository.Add(club);
        }




    }
}
