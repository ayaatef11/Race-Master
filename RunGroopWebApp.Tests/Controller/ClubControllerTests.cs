
using RunGroop.Application.ViewModels;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;
using RunGroop.Repository.Repository;
using RunGroopWebApp.Controllers;

namespace RunGroopWebApp.Tests.Controller
{
    public class ClubControllerTests
    {
        private ClubController _clubController;
        private ClubRepository _clubRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;
        public ClubControllerTests()
        {
            _clubRepository = A.Fake<ClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();
            //_clubController = new ClubController(_clubRepository, _photoService);
        }

        [Fact]
        public void ClubController_RunningClubsByStateDirectory_ReturnsSuccess()
        {
            var clubs = A.Fake<IEnumerable<Club>>();
            //why
            A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);
            var result = _clubController.RunningClubsByStateDirectory();
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]//any testable method must be fact 
        public void ClubController_Detail_ReturnsSuccess()
        {
            //arrange (initialize the variables)-act (call the method )-assert(check if expected equal actual) 
            //take the edge cases 
            int id = 1;
            var club = A.Fake<Club>();//create a fake instance 
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);//sets the fake behaviour 
            ClubDetailsViewModel vv = new()
            {
                Id = id,
                RunningClub = "RunningClub"
            };
            var result = _clubController.DetailClub(vv);
           
            result.Should().BeOfType<Task<IActionResult>>();//check the result 
        }


    }
}
