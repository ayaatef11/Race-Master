using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RunGroop.Application.Extensions;
using RunGroop.Data.Data;
using RunGroop.Data.Models.Data;
using Microsoft.Extensions.Configuration;
using RunGroopWebApp.Data.Enum;

namespace RunGroopWebApp.Scraper.Services
{
    public class MeetupScraper
    {
        private IWebDriver _driver;
        private readonly IConfiguration _configuration;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public MeetupScraper()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())  // Get the current directory
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = _configuration.GetConnectionString("RunGroopDb");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            _dbContextOptions = optionsBuilder.Options;

            var options = new ChromeOptions();
            // Uncomment the next line if you want to run in headless mode
            // options.AddArgument("--headless");
            _driver = new ChromeDriver(options);
        }

        public void Run()
        {
            try
            {
                IterateOverRunningClubs();
            }
            finally
            {
                // Close the browser after all processing is done
                _driver.Quit();
            }
        }

      
        public void IterateOverRunningClubs()
        {
            try
            {
                string  url = "https://www.meetup.com/find/?keywords=running%20club";
                

                _driver.Navigate().GoToUrl(url);

                // Wait for the page to load
                //Thread.Sleep(5000);

                //                var clubCards = _driver.FindElements(By.CssSelector(".revamped-group-card"));
                //var clubCards = _driver.FindElements(By.CssSelector("div.grid.grid-cols-1"));
                var gridContainer = _driver.FindElement(By.ClassName("grid"));

                // Then find all club cards within this container
                var clubCards = gridContainer.FindElements(By.CssSelector("div.flex.w-full.flex-col.items-center"));

                foreach (var card in clubCards)
                {
                    try
                    {
                        string title = card.FindElement(By.CssSelector("h3")).Text;
                        string location = card.FindElement(By.CssSelector(".text-sm.text-ds-neutral500")).Text;
                        string[] locationParts = location.Split(',');
                        string clubCity = locationParts[0].Trim();
                        string clubState = locationParts.Length > 1 ? locationParts[1].Trim() : "";

                        string detailUrl = card.FindElement(By.TagName("a")).GetAttribute("href");

                        string imageUrl = "";
                        try
                        {
                            var imageElement = card.FindElement(By.CssSelector("img.size-full.object-cover"));
                            imageUrl = imageElement.GetAttribute("src");
                        }
                        catch (NoSuchElementException)
                        {
                            Console.WriteLine("No image found for this club.");
                        }

                        Console.WriteLine($"Title: {title}");
                        Console.WriteLine($"City: {clubCity}");
                        Console.WriteLine($"State: {clubState}");
                        Console.WriteLine($"URL: {detailUrl}");
                        Console.WriteLine($"Image URL: {imageUrl}");

                        // Store current URL to return to later
                        string currentUrl = _driver.Url;

                        // Navigate to the detailed page
                        _driver.Navigate().GoToUrl(detailUrl);

                        // Wait for the detail page to load
                        Thread.Sleep(3000);

                        string description = "";
                        try
                        {
                            var descriptionElement = _driver.FindElement(By.CssSelector(".break-words.utils_description_BIOCA"));
                            description = descriptionElement.Text;
                            Console.WriteLine("Description: " + description);
                        }
                        catch (NoSuchElementException)
                        {
                            Console.WriteLine("Description not found.");
                        }

                        Console.WriteLine("-------------------");

                        // Create a new club record
                        var club = new Club()
                        {
                            Title = title,
                            Address = new Address()
                            {
                                City = clubCity,
                                State = clubState
                            },
                            Image = imageUrl,
                            Description = description,
                            ClubCategory = ClubCategory.City
                        };

                        // Save to database if it doesn't already exist
                        using (var context = new ApplicationDbContext(_dbContextOptions))
                        {
                            if (!context.Clubs.Any(c => c.Title == club.Title))
                            {
                                context.Clubs.Add(club);
                                context.SaveChanges();
                                Console.WriteLine($"Added club: {club.Title}");
                            }
                            else
                            {
                                Console.WriteLine($"Club already exists: {club.Title}");
                            }
                        }

                        // Navigate back to the search results
                        _driver.Navigate().GoToUrl(currentUrl);

                        // Wait for the search page to reload
                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing club card: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing ");
            }
        }
    }
}