using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.IO;

namespace RunGroopWebApp.Controllers;

[Route("api/[controller]/[action]")]
[Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
public class ReportViewerController : Controller
{
    private readonly IMemoryCache cache;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ReportViewerController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
    {
        cache = memoryCache;
        _hostingEnvironment = hostingEnvironment;
    }

   /* [HttpPost]
    public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
    {
        return ReportHelper.ProcessReport(jsonArray, this, cache);
    }*/

    [NonAction]
    public void OnInitReportOptions(ReportViewerOptions reportOption)
    {
        string basePath = Path.Combine(_hostingEnvironment.WebRootPath, "Resources");
        string reportPath = Path.Combine(basePath, reportOption.ReportModel.ReportPath);

        if (!System.IO.File.Exists(reportPath))
        {
            throw new FileNotFoundException("Report file not found at " + reportPath);
        }

        using (FileStream fileStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read))
        {
            MemoryStream reportStream = new MemoryStream();
            fileStream.CopyTo(reportStream);
            reportStream.Position = 0;
            reportOption.ReportModel.Stream = reportStream;
        }
    }

    [NonAction]
    public void OnReportLoaded(ReportViewerOptions reportOption)
    {
        // Add logic or leave empty
    }

   /* [ActionName("GetResource")]
    [AcceptVerbs("GET")]
    public object GetResource(ReportResource resource)
    {
        return ReportHelper.GetResource(resource, this, cache);
    }*/

    //[HttpPost]
  /*  public object PostFormReportAction()
    {
        return ReportHelper.ProcessReport(null, this, cache);
    }*/
}
