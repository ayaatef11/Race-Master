using BoldReports.Web.Reportviewer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using System.Text.Json.Nodes;
using CloudinaryDotNet.Actions;

namespace CoreAPIService.Controllers;

[Route("api/[controller]/[action]")]
[Microsoft.AspNetCore.Cors.EnableCors("AllowAllorigins")]

public class ReportViewerController : Controller

{
    private IMemoryCache cache;

    private IWebHostEnvironment _hostingEnvironment;

    public ReportViewerController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
    {

        cache = memoryCache;
        _hostingEnvironment = hostingEnvironment;
    }
    [HttpPost]
    public object PostReportAction([FromBody] Dictionary<string, object> jsonArray) {

        return ReportHelper.ProcessReport(jsonArray, this, this.cache);
    }

    [NonAction]
     public void OnInitReportOptions(ReportViewerOptions reportOption) {

        string basePath = Path.Combine(_hostingEnvironment.WebRootPath, "Resources");
        string reportPath = Path.Combine(basePath, reportOption.ReportModel.ReportPath);
        FileStream fileStream = new FileStream(reportPath, FileMode.Open, FileAccess.Read);
        MemoryStream reportStream = new MemoryStream();
        fileStream.CopyTo(reportStream);
        reportStream.Position = 0;
        fileStream.Close();
        reportOption.ReportModel.Stream = reportStream;
     }

    [NonAction]
public void OnReportLoaded(ReportViewerOptions reportOption)

}

[ActionName("GetResource")]
[AcceptVerbs("GET")]
public object GetResource(ReportResource resource) {

    return ReportHelper.GetResource(resource, this, cache);
}
[HttpPost]

public object PostFormReportAction() { 

return ReportHelper.ProcessReport(null, this,cache);
}
