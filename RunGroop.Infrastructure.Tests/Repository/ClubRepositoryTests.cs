
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using RunGroop.Data.Data;
//using RunGroop.Data.Models.Data;
//using RunGroop.Repository.Repository;
//using RunGroopWebApp.Data.Enum;
//using RunGroopWebApp.Repository;
//using Xunit;

//namespace RunGroopWebApp.Tests.Repository
//{
//    public class ClubRepositoryTests
//    {
//        private async Task<ApplicationDbContext> GetDbContext()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Generates a unique DB each time
//                .Options;

//            var databaseContext = new ApplicationDbContext(options);

//            if (await databaseContext.Clubs.CountAsync() == 0) // Correct condition
//            {
//                var clubs = new List<Club>(); // Use a list to batch insert

//                for (int i = 0; i < 10; i++)
//                {
//                    clubs.Add(new Club()
//                    {
//                        Title = "Running Club 1",
//                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                        Description = "This is the description of the first club",
//                        ClubCategory = ClubCategory.City,
//                        Address = new Address()
//                        {
//                            Street = "123 Main St",
//                            City = "Charlotte",
//                            State = "NC"
//                        }
//                    });
//                }

//                await databaseContext.Clubs.AddRangeAsync(clubs); // Add all at once
//                await databaseContext.SaveChangesAsync(); // Save changes once
//            }

//            return databaseContext;
//        }

//        [Fact]
//        public async Task ClubRepositoryGetByIdAsyncReturnsClub()
//        {
//            var id = 1;
//            var dbContext = await GetDbContext();
//            var clubRepository = new ClubRepository(dbContext);
//            var result = clubRepository.GetByIdAsync(id);
//            result.Should().NotBeNull();
//            result.Should().BeOfType<Task<Club>>();
//        }

//        [Fact]
//        public async Task ClubRepositoryGetAllReturnsList()
//        {
//            var dbContext = await GetDbContext();
//            var clubRepository = new ClubRepository(dbContext);
//            var result = await clubRepository.GetAll();
//            result.Should().NotBeNull();
//            result.Should().BeOfType<List<Club>>();
//        }

      

//        [Fact]
//        public async Task ClubRepositoryGetCountAsyncReturnsInt()
//        {
//            var club = new Club()
//            {
//                Title = "Running Club 1",
//                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                Description = "This is the description of the first cinema",
//                ClubCategory = ClubCategory.City,
//                Address = new Address()
//                {
//                    Street = "123 Main St",
//                    City = "Charlotte",
//                    State = "NC"
//                }
//            };
//            var dbContext = await GetDbContext();
//            var clubRepository = new ClubRepository(dbContext);
//            clubRepository.Add(club);
//            var result = await clubRepository.GetCountAsync();
//            result.Should().Be(1);
//        }

//        [Fact]
//        public async Task ClubRepositoryGetAllStatesReturnsList()
//        {
//            var dbContext = await GetDbContext();
//            var clubRepository = new ClubRepository(dbContext);
//            var result = await clubRepository.GetAllStates();
//            result.Should().NotBeNull();
//            result.Should().BeOfType<List<State>>();
//        }

//        [Fact]
//        public async Task ClubRepositoryGetClubsByStateReturnsList()
//        {
//            var state = "NC";
//            var club = new Club()
//            {
//                Title = "Running Club 1",
//                Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
//                Description = "This is the description of the first cinema",
//                ClubCategory = ClubCategory.City,
//                Address = new Address()
//                {
//                    Street = "123 Main St",
//                    City = "Charlotte",
//                    State = "NC"
//                }
//            };
//            var dbContext = await GetDbContext();
//            var clubRepository = new ClubRepository(dbContext);
//            clubRepository.Add(club);
//            var result = await clubRepository.GetClubsByState(state);
//            result.Should().NotBeNull();
//            result.Should().BeOfType<List<Club>>();
//            result.First().Title.Should().Be("Running Club 1");
//        }
//    }
//}
