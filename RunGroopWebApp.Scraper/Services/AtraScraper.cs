
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RunGroop.Data.Data;
using RunGroop.Data.Models.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Scraper.Extensions;
using RunGroopWebApp.Scraper.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace RunGroopWebApp.Scraper.Services
{
    public class AtraScraper : IAtraScraper
    {
        private IWebDriver _driver;
        private readonly IConfiguration _configuration;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public AtraScraper()
        {
            _configuration = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())  // Get the current directory
                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .Build();
            var connectionString = _configuration.GetConnectionString("RunGroopDb");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            _dbContextOptions = optionsBuilder.Options;
            _driver = new ChromeDriver();
        }

        public void Run()
        {
            IterateOverRaceElements();
        }

        public IReadOnlyCollection<IWebElement> GetElements()
        {
            return _driver.FindElements(By.CssSelector("span[itemprop='name']"));
        }

        public void IterateOverRaceElements()
        {
                _driver.Navigate().GoToUrl("https://trailrunner.com/race-calendar/");
                Thread.Sleep(3000); // استنى شويه لحد ما الصفحة تفتح

                // كل صف (tr) يمثل حدث
                var eventRows = _driver.FindElements(By.CssSelector("tr[itemtype='http://schema.org/Event']"));

                foreach (var row in eventRows)
                {
                    var cells = row.FindElements(By.TagName("td"));

                    
                        string date = cells[0].Text.Trim();

                        // اسم الحدث والرابط
                        var linkElement = cells[1].FindElement(By.TagName("a"));
                        string name = linkElement.Text.Trim();
                        string link = linkElement.GetAttribute("href");

                        // المسافات - ممكن تكون جوه ul > li
                        var distancesList = cells[2].FindElements(By.CssSelector("ul li"));
                        string distances = string.Join(", ", distancesList.Select(d => d.Text.Trim()));

                        // النوع (Ultra / Trail ...)
                        string type = cells[3].Text.Trim();

                        // الموقع
                        string location = cells[4].Text.Trim();

                        Console.WriteLine($"Date: {date}\nName: {name}\nLink: {link}\nDistances: {distances}\nType: {type}\nLocation: {location}");
                        Console.WriteLine("--------------------------------------------------");
                Race race = new Race()
                {
                    Date = date,
                    Name = name,
                    Distance = distances,
                    Type = type,
                    Location = new Address()
                    {
                        City = location
                    }

                };
                using (var context = new ApplicationDbContext(_dbContextOptions))
                {
                    if (!context.Races.Any(c => c.Name == race.Name))
                    {
                        context.Races.Add(race);
                        context.SaveChanges();
                    }
                }
            }
               

                _driver.Quit();
            }
        }

}


/*using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

IWebDriver driver = new ChromeDriver();
driver.Url = "https://www.google.com";
driver.FindElement(By.Id("APjFqb")).SendKeys("ID Name ClassName");//put the key className in the search bar
driver.FindElement(By.Id("APjFqb")).SendKeys(Keys.Enter);//press enter to search for the key entered
System.Threading.Thread.Sleep(5000);
//driver.FindElement(By.Name("q")).SendKeys("Name");
//driver.FindElement(By.ClassName()).SendKeys("ClassName");
//driver.FindElement(By.XPath("")).Click();?????????
//find elements, find an element by its index
//driver.FindElements(By.ClassName("r"))[2].Click();
//WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));
//wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("r.")));
//checkbox
driver.Url = "";
driver.FindElements(By.Name("status"))[2].Click();//check the checkbox
driver.FindElements(By.Name("status"))[2].Click();//uncheck the checkbox
//radio button to  locate a radio button and select a specific selection



//drop down list

//upload and download

//unit test helps in the automation test



//driver.Close();*/