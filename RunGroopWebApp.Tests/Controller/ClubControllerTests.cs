using RunGroop.Data.Interfaces.Repositories;
using RunGroop.Data.Interfaces.Services;
using RunGroop.Data.Models.Data;
using RunGroopWebApp.Controllers;

namespace RunGroopWebApp.Tests.Controller
{
    public class ClubControllerTests
    {
        private ClubController _clubController;
        private IClubRepository _clubRepository;
        private IPhotoService _photoService;
        private IHttpContextAccessor _httpContextAccessor;
        public ClubControllerTests()
        {
            _clubRepository = A.Fake<IClubRepository>();
            _photoService = A.Fake<IPhotoService>();
            _httpContextAccessor = A.Fake<HttpContextAccessor>();
            _clubController = new ClubController(_clubRepository, _photoService);
        }

        [Fact]
        public void ClubController_Index_ReturnsSuccess()
        {
            var clubs = A.Fake<IEnumerable<Club>>();
            A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);
            var result = _clubController.Index();
            result.Should().BeOfType<Task<IActionResult>>();
        }

        [Fact]//any testable method must be fact 
        public void ClubController_Detail_ReturnsSuccess()
        {
            //arrange (initialize the variables)-act (call the method )-assert(check if expected equal actual) 
            //take the edge cases 
            var id = 1;
            var club = A.Fake<Club>();//create a fake instace 
            A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);//sets the fake behaviour 
            
            var result = _clubController.DetailClub(id, "RunningClub");
           
            result.Should().BeOfType<Task<IActionResult>>();//check the result 
        }


    }
}
