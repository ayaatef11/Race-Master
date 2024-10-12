using RunGroop.Application.Mapping;
using RunGroop.Data.Models.Data;

namespace RunGroopWebApp.ViewModels
{
    public class ClubDetailsViewModel:IMapFrom<Club>
    {
       public int Id { get; set; }
        public string RunningClub { get; set; } = string.Empty;
    }
}