
using RunGroopWebApp.Scraper.Services;
Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

MeetupScraper scraper = new MeetupScraper();
//AtraScraper scraper1 = new AtraScraper();
scraper.Run();
