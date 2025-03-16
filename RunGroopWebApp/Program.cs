using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using RunGroop.Data.Data;
using RunGroop.Data.Models.Identity;
using RunGroopWebApp.Services.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;
using RunGroop.Application.Extensions;
using RunGroop.Application.MiddleWares;
using RunGroop.Data.Data.Seeding;
using RunGroopWebApp.Services.Localization;
using System.Globalization;
using RunGroop.Application.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

try
{
    Log.Information("Starting a web application");

    var builder = WebApplication.CreateBuilder(args);
    var myCorsPolicy = "MyCorsPolicy";

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(myCorsPolicy, builder =>
        {
            builder.AllowAnyOrigin().WithExposedHeaders("Custom-Header");
        });
    });

    builder.Services.AddHostedService<BackGroundWorkerService>();
    builder.Services.AddControllersWithViews();

    builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
    builder.Host.ConfigureSerilog();

    builder.Services.AddHangfire(config =>
        config.UseSimpleAssemblyNameTypeSerializer()
              .UseRecommendedSerializerSettings()
              .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnections"))
    );

    builder.Services.AddHangfireServer();

    builder.Services.AddDbContext<ApplicationDbContext>(opts =>
        opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections"),
        options => options.MigrationsAssembly("RunGroopWebApp"))
    );

    builder.Services.AddDistributedMemoryCache();
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromSeconds(10);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    });

    builder.Services.AddQuartz(opt =>
    {
        opt.UsePersistentStore(s =>
        {
            s.UseSqlServer(builder.Configuration.GetSection("Quartz:dataSource:default:QuartzConnection").Value);
            s.UseProperties = true;
            s.UseClustering(); 
            s.UseJsonSerializer(); 

        });

        var jobKey = JobKey.Create("LoggingJob");

        opt.AddJob<LogBackgroundJob>(jobKey)
           .AddTrigger(trigger =>
           {
               trigger.ForJob(jobKey)
                      .StartAt(DateBuilder.FutureDate(10, IntervalUnit.Second))
                      .WithSimpleSchedule(s => s.WithIntervalInSeconds(2).RepeatForever())
                      .EndAt(DateBuilder.FutureDate(5, IntervalUnit.Hour));
           });
    });

    builder.Services.AddQuartzHostedService(options =>
    {
        options.WaitForJobsToComplete = true;
    });



    builder.Services.AddIdentity<AppUser, IdentityRole>()
           .AddDefaultTokenProviders()
           .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddMemoryCache(opt => opt.SizeLimit = 1024);
    builder.Services.AddLocalization();
    builder.Services.AddMvc()
           .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
           .AddDataAnnotationsLocalization();

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("ar-EG"),
            new CultureInfo("de-DE")
        };

        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });

    var app = builder.Build();

    app.UseCors(myCorsPolicy);
    app.UseStaticFiles();
    app.UseSession();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGet("/set-cookie", (HttpContext httpContext) =>
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(1)
        };

        httpContext.Response.Cookies.Append("auth-token", "your-token-value", cookieOptions);
        return Results.Ok("HttpOnly cookie has been set.");
    });

    app.MapGet("/get-cookie", (HttpContext httpContext) =>
    {
        var authToken = httpContext.Request.Cookies["auth-token"];
        return authToken != null
            ? Results.Ok($"Your auth token is: {authToken}")
            : Results.Ok("No auth token found.");
    });

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
