/*
using GMap.NET;
using GMap.NET.MapProviders;
using OpenQA.Selenium.Chromium;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RunGroopWebApp.Scraper.Services
{
    public class DynamoScraper
    {
        private static IWebDriver driver;
        private GMapControl map;  // Ensure GMapControl is initialized properly in the constructor or elsewhere.
        private TextBox txtLat;   // Placeholder for TextBox (needs proper assignment in real context)
        private TextBox txtLong;  // Placeholder for TextBox (needs proper assignment in real context)
        private List<PointLatLng> _points; // Points to define routes/polygons, ensure it's initialized correctly

        public DynamoScraper()
        {
            // Initialize GMapControl or assign it from your UI context
            map = new GMapControl();
            GMapProviders.GoogleMap.ApiKey = ""; // Add your API Key

            // Parse latitude and longitude
            double lat = Convert.ToDouble(txtLat.Text);
            double lng = Convert.ToDouble(txtLong.Text); // Fixed typo (missing semicolon)
            map.Position = new PointLatLng(lat, lng);
            map.MinZoom = 5; // Minimum Zoom Level
            map.MaxZoom = 100; // Maximum Zoom Level
            map.Zoom = 10;

            // Adding a marker
            PointLatLng point = new(lat, lng);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green);

            // Create Overlay for markers
            GMapOverlay markers = new ("markers");
            markers.Markers.Add(marker);
            map.Overlays.Add(markers);

            // Ensure _points is initialized correctly with at least two points
            if (_points == null || _points.Count < 2)
            {
                Console.WriteLine("Error: _points must have at least two coordinates.");
                return;
            }

            // Create a route
            var route = GoogleMapProvider.Instance.GetRoute(_points[0], _points[1], false, false, 14);
            var r = new GMapRoute(route.Points, "My Route");

            // Add the route to map overlays
            var routes = new GMapOverlay("routes");
            routes.Routes.Add(r);
            map.Overlays.Add(routes);

            // Define a polygon
            var polygon = new GMapPolygon(_points, "My Area")
            {
                Stroke = new Pen(Color.DarkGreen, 2),
                Fill = new SolidBrush(Color.BurlyWood)
            };

            // Add polygon overlay
            var polygons = new GMapOverlay("polygons");
            polygons.Polygons.Add(polygon);
            map.Overlays.Add(polygons);

            // Setup Selenium WebDriver with ChromeOptions
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless"); // Running in headless mode
            ChromeDriver driver = new(options);

            driver.NetworkConditions = new ChromiumNetworkConditions
            {
                DownloadThroughput = 25 * 1000,
                UploadThroughput = 10 * 1000,
                Latency = TimeSpan.FromMilliseconds(1)
            };

            // Navigate to ReverbNation and interact
            driver.Navigate().GoToUrl("https://www.reverbnation.com/");
            var discoverButton = driver.FindElement(By.Id("menu-item-discover"));
            discoverButton.Click();

            // Fetch elements with specific class name
            var collections = driver.FindElements(By.ClassName("card_contents"));
            foreach (var collection in collections)
            {
                Console.WriteLine(collection.Text);
            }
        }

        // Method to find elements with retries
        static IReadOnlyCollection<IWebElement> FindElements(By by)
        {
            while (true)
            {
                var elements = driver.FindElements(by);
                if (elements.Count > 0)
                    return elements;

                Thread.Sleep(10);
            }
        }
    }
}
*/