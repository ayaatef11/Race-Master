
using AngleSharp.Dom;
using CloudinaryDotNet.Actions;
using DocumentFormat.OpenXml.Bibliography;
using RunGroop.Application.Extensions;
using RunGroop.Data.Models.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Scraper.Data;

namespace RunGroopWebApp.Scraper.Services
{
    public class MeetupScraper
    {
        private IWebDriver _driver;
        public MeetupScraper()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            _driver = new ChromeDriver();//open a new web browser
        }


        public void Run()
        {
            GetListOfCityAndState();
        }

        public void GetListOfCityAndState()
        {
            int batchSize = 100;
            int currentBatch = 0;
            bool done = false;
            // while(!done)
            using (var context = new ScraperDBContext())
            {
                IterateOverRunningClubs("city.StateCode.ToLower()", "city.CityName.ToLower()");

                var cities = context.Cities.OrderBy(x => x.Id).Skip(currentBatch++ * batchSize).Take(batchSize).ToList();
                foreach (var city in cities)
                {
                    IterateOverRunningClubs(city.StateCode.ToLower(), city.CityName.ToLower());
                    if (city.Id == 40000)
                    {
                        done = true;
                    }
                }
            }
        }


        public void IterateOverRunningClubs(string state, string city)
        {
            try
            {
                //make a web scraper for google search results 
                //navigate for the page 
                _driver.Navigate().GoToUrl($"https://www.meetup.com/find/?suggested=true&source=GROUPS&keywords=running%20club&location=us--{state}--{city}");
                //find element by a shape of path
                //get the name of the card
                var pageElements = _driver.FindElements(By.CssSelector("h3[data-testid='group-card-title']"));//identify the controls of the page 
                //perform operations 
                for (int i = 0; i < pageElements.Count; i++)
                {
                    try
                    {
                        pageElements = _driver.FindElements(By.CssSelector("h3[data-testid='group-card-title']"));
                        var element = pageElements.ElementAt(i);
                        var placeholder = element.Text;
                        var placeholder2 = element.Text.Contains("run", System.StringComparison.CurrentCultureIgnoreCase);
                        if (element.Text.Contains("run", System.StringComparison.CurrentCultureIgnoreCase))
                        {
                            element.Click();
                            element.SendKeys("RunGroop");
                            element.Submit();
                            var club = new Club()
                            {
                                Title = _driver.FindElement(By.CssSelector("a[class='groupHomeHeader-groupNameLink']")).Text ?? "",
                                Description = _driver.FindElement(By.CssSelector("p[class='group-description _groupDescription-module_description__3qvYh margin--bottom']")).Text ?? "",
                                Address = new Address()
                                {
                                    State = state.ToUpper(),
                                    City = city.FirstCharToUpper()
                                },
                                ClubCategory = ClubCategory.City
                            };
                            using (var context = new ScraperDBContext())
                            {
                                if (!context.Clubs.Any(c => c.Title == club.Title))
                                {
                                    context.Clubs.Add(club);
                                    context.SaveChanges();
                                }
                            }
                            _driver.Navigate().Back();
                        }

                    }

                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                        _driver.Navigate().Back();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public static string GetText(IWebDriver driver, string element, string elementtype)
        {

            if (elementtype == "Id")
                return driver.FindElement(By.Id(element)).GetAttribute("value");
            if (elementtype == "Name")
                return driver.FindElement(By.Name(element)).GetAttribute("value");
            else return String.Empty;
        }
       /* public static string GetTextFromDDL(IWebDriver driver, string element, string elementtype)
        {

            if (elementtype == "Id")
               // return new SelectElement(driver.FindElement(By.Id(element))).AllSelectedOptions.SingleOrDefault().Text;
            if (elementtype == "Name")
              //  return new SelectElement(driver.FindElement(By.Name(element))).AllSelectedOptions.SingleOrDefault().Text;
            else return String.Empty;
        }*/

        public static void EnterText(IWebDriver driver, string element, string value, string elementtype)
        {

            if (elementtype == "Id")
                driver.FindElement(By.Id(element)).SendKeys(value);
            if (elementtype == "Name")
                driver.FindElement(By.Name(element)).SendKeys(value);
        }
        public static void Click(IWebDriver driver, string element, string elementtype)
        {

            if (elementtype == "Id")
                driver.FindElement(By.Id(element)).Click();
            if (elementtype == "Name")
                driver.FindElement(By.Name(element)).Click();
        }
    }
}