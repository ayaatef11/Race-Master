using Azure;
using MediatR;
using RunGroop.Data.Interfaces.Repositories;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Queries;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Handlers
{
    public class GetAllDriversHandler(IClubRepository _clubRepository) : IRequestHandler<GetAllClubsQuery, IndexClubViewModel>
    {

        public Task<IndexClubViewModel> Handle(GetAllClubsQuery request, CancellationToken cancellationToken, int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            // if category is -1 (All) dont filter else filter by selected category
            var clubs = category switch
            {
                -1 => await _clubRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _clubRepository.GetClubsByCategoryAndSliceAsync((ClubCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _clubRepository.GetCountAsync(),
                _ => await _clubRepository.GetCountByCategoryAsync((ClubCategory)category),
            };

            var clubViewModel = new IndexClubViewModel
            {
                Clubs = clubs,
                Page = page,
                PageSize = pageSize,
                TotalClubs = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };
        }
    }
}
