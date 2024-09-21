using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using RunGroop.Data.Data;
using RunGroop.Data.Models;
using RunGroop.Data.Models.Identity;
using RunGroop.Infrastructure;
using RunGroopWebApp.Data;
using RunGroopWebApp.Extensions;
using RunGroopWebApp.Services.Services;
using GraphQL;
using static RunGroop.Data.Models.ClubQuery;
using RunGroop.Data.Models.Localization;
using RunGroop.Data.Helpers.Settings;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events; // Optional, for handling log event levels if needed
using System;
using MediatR;
using RunGroopWebApp.Behaviors;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Options;
using RunGroopWebApp;
using RunGroop.Infrastructure.Settings;
using RunGroopWebApp.Services.interfaces;
using RunGroopWebApp.Clients;
using Microsoft.Owin.Builder;
using FluentValidation;
using RunGroop.Data;
/*The cross-origin resource sharing (CORS) specification prescribes header content exchanged between
web servers and browsers that restricts origins for web resource requests outside of the origin domain.
The CORS specification identifies a collection of protocol headers of which Access-Control-
Allow-Origin is the most significant. This header is returned by a server when a website requests a
cross-domain resource, with an Origin header added by the browser.

For example, suppose a website with origin normal-website. com causes the following cross-
domain request:

GET /data HTTP/1.1

Host: robust-website.com

Origin : https://normal-website.com

The server on robust-website.com returns the following response:

HTTP/1.1 200 OK

..

Access-Control-Allow-Origin: https://normal-website.com

The browser will allow code running on normal-website. com to access the response because the
origins match.

The specification of Access-Control-Allow-Origin allows for multiple origins, or the value
* . However, no browser supports multiple origins and there are restrictions on
the use of the wildcard *

null , or the wildcard*/

//in multi tenancy you host the application one time and the organizations can access it with different databases 
/*Example: Sending Push Notifications from .NET using FCM
Install Required Packages: Install the Firebase Admin SDK NuGet package in your .NET project to interact with FCM.

bash
Copy code
dotnet add package FirebaseAdmin
Initialize Firebase in .NET: Use the Firebase Admin SDK to send notifications.

csharp
Copy code
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

// Initialize Firebase Admin SDK (use the downloaded credentials file)
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("path/to/serviceAccountKey.json")
});
Send Push Notification:

Create a method to send a push notification using the FCM Admin SDK.

csharp
Copy code
public async Task SendPushNotification(string deviceToken, string title, string body)
{
    // Construct the message to be sent
    var message = new Message()
    {
        Token = deviceToken,
        Notification = new Notification()
        {
            Title = title,
            Body = body
        }
    };

    // Send the message via Firebase
    string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
    Console.WriteLine("Successfully sent message: " + response);
}
deviceToken: This is the FCM token for the device to which you want to send the notification.
title: Title of the push notification.
body: Message body of the push notification.
Trigger Notification from Web API:

You can create an API endpoint that triggers the push notification when certain events occur, such as when a new order is placed.

csharp
Copy code
[HttpPost]
[Route("api/send-notification")]
public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
{
    // Replace this with actual logic to retrieve the device token
    var deviceToken = request.DeviceToken;
    
    await SendPushNotification(deviceToken, request.Title, request.Body);
    
    return Ok("Notification Sent");
}
The NotificationRequest class could look like:

csharp
Copy code
public class NotificationRequest
{
    public string DeviceToken { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}
Web Push Notifications in .NET
For browser notifications, you can use Web Push via the Web Push Protocol.

Install Web Push Library: Use the WebPush NuGet package for sending notifications to web browsers.

bash
Copy code
dotnet add package Lib.Net.WebPush
Set Up Public and Private VAPID Keys: Generate VAPID keys that are used for identifying the sender of the push notification.

bash
Copy code
npm install -g web-push
web-push generate-vapid-keys
You will get a public and private key. Store these keys securely and use them in your server code.

Send Web Push Notification in .NET:

csharp
Copy code
using Lib.Net.Http.WebPush;

public async Task SendWebPushNotification(string endpoint, string p256dh, string auth, string payload)
{
    var pushSubscription = new PushSubscription(endpoint, p256dh, auth);

    var vapidDetails = new VapidDetails("mailto:example@example.com", "PUBLIC_VAPID_KEY", "PRIVATE_VAPID_KEY");

    var webPushClient = new WebPushClient();

    try
    {
        await webPushClient.SendNotificationAsync(pushSubscription, payload, vapidDetails);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error sending web push notification: " + ex.Message);
    }
}
In this method:

endpoint, p256dh, and auth are obtained from the browser's push subscription.
payload is the message content to be sent.
Send Notification on Event: Similar to FCM, you can create an API endpoint that triggers Web Push notifications.

Push Notification Flow Summary
User Device/Browser: Registers and subscribes for notifications (obtains device token or push subscription).
Server (.NET Web API): Manages notifications and communicates with services like FCM or Web Push to send notifications to the device or browser.
Push Notification Service (e.g., FCM or Web Push): Delivers notifications to the respective client.*/

var builder = WebApplication.CreateBuilder(args);
//add backend services
//glopalization means the application supports many languages and localization means the application adapts to the change of the language
var myCorsPolicy = "MyCorsPolicy";
builder.Services.AddCors(options => { 

options.AddPolicy(myCorsPolicy,
builder =>
{

    builder.AllowAnyOrigin()
    .WithExposedHeaders(CustomHeaderNames.CustomAddName);
});
});
builder.Services.AddHostedService<BackGroundWorkerService>();
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationServices();
//builder.Services.AddConfigurationServices(builder.Configuration);
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//when you need to separate the migration in the webapplication and the context in the repository then you define the asembly for the migration and this is the name of the project
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections"),
options => options.MigrationsAssembly("RunGroopWebApp")));
//you can apply commands in the migration like for example in the up method you call migrationbuilder.sql("here you write the command ")

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});




//builder.Services.AddGraphQL(opt => opt.EnableMetrics = false).AddSystemTextJson();
/*
Host.CreateDefaultBuilder(args)
.ConfigureWebHostDefaults(webBuilder =>*/

/*webBuilder.UseKestrel(options => {
    options.Listen(IPAddress.Any, 5000);// HTTP on port 5000
    options.Listen(IPAddress.Any, 5001, listenOptions => 
    listenOptions.UseHttps("path-to-cert.pfx", "cert-password") // HTTPS

);*/

/*
builder.Services.Configure<ApiBehaviorOptions>(options =>
{

    options.InvalidModelStateResponseFactory = actionContext =>

    var errors = actionContext.ModelState;
    }*/
//generate tokens for common user account tasks like password resets, email confirmation, and multi-factor authentication (MFA).

/*What Happens Under the Hood:
When you call AddDefaultTokenProviders, it registers several token providers:

Data Protection Token Provider: This is the default token provider used for generating tokens for email confirmation, password reset, etc.
Phone Number Token Provider: Used specifically for phone verification and SMS-based MFA.
Authenticator Token Provider: Used for generating tokens compatible with authenticator apps.
Email and Password Reset Token Providers: Generate tokens for user actions that modify account data or access.
Security Considerations:
Expiration: Tokens have an expiration time that is set to ensure security. For example, a password reset token might expire after a certain number of hours.

Validation: Tokens are validated against the user’s data, ensuring they are not reused and are specific to the request they were generated for.

Custom Token Providers: If the default token providers don't meet your needs, you can also create and configure custom token providers by implementing IUserTwoFactorTokenProvider<TUser>.*/


builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>,typeof(ValidationBehavior<,>)))
builder.Services.AddIdentity<AppUser, IdentityRole>().AddDefaultTokenProviders()//.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie();
builder.Services.AddHttpClient("companiesClient", config =>
{
    config.BaseAddress = new Uri("https://localhost:5001/api/");
    config.Timeout = new TimeSpan(0, 0, 30);
    config.DefaultRequestHeaders.Clear();
});
builder.Services.AddMemoryCache(
    opt=>opt.SizeLimit=1024);
//note
/*when we set a size limit for the cache in the options , we must specicfy a size for all the cache entries
 similarly , if no cache size limit is set , the size set on indvidual cache entries will be ignored
once the cache reaches its limit , it doesn't remove the oldest entry to make room for new entries  
the cache items will be removed automatically in certain senarios :
1- when the application server is running short of memory
2- once we set the sliding expiration or the absolute expiration 
if we try to add the cache entry with the same key;
 it is thread safe it is borne to race conditions 
 */
builder.Services.AddHttpClient<CompaniesClient>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddSingleton<ISchema, ClubSchema>();
//* builder.Services.Configure<ApplicationOptions>(builder.Configuration.GetSection(nameof(ApplicationOptions)));
builder.Services.ConfigureOptions<ApplicationOptionsSetup>();
//builder.Services.AddGraphQL(opt => opt.EnableMetrics = false).AddSystemTextJson();
builder.Services.AddSignalR();
builder.Services.AddCors();

builder.Services.AddMvc()//to use the localizer inside the view 
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {//to use it with the data annotations when you need to display the attribute in some text 
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(JsonStringLocalizerFactory));
    });

//(**configurations
//accept language tells you the languages that your browser supports 
builder.Services.AddScoped<TenantSettings>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG"),
        new CultureInfo("de-DE")
    };
    //that was to take the first culture which is engilsh to be the default but we need our application to make the default language based on the browsser language 
    //options.DefaultRequestCulture = new RequestCulture(culture: supportedCultures[0], uiCulture: supportedCultures[0]);// we used the middleware instead
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));
//you can use this code to read the value for the configuration
TenantSettings options = new();
builder.Configuration.GetSection(nameof(TenantSettings)).Bind(options);
builder.Services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer());
var defaultDbProvider = options.Defaults.DBProvider;
if (defaultDbProvider.ToLower() == "mssql")
{

    foreach (var tenant in options.Tenants) { 

        var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;

    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    dbContext.Database.SetConnectionString(connectionString);
        if (dbContext.Database.GetPendingMigrations().Any())

            dbContext.Database.Migrate();//allows to not use the update-database

    }

}
    builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    await  UsersSeed.SeedUsersAsync(app, "ayabadrin667@gmail.com", "Admin");
    await  RolesSeed.SeedRolesAsync(app);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.MapGet("Options", (IOptions<ApplicationOptions>options,
    IOptionsSnapshot<ApplicationOptions>optionsSnapShot,
    IOptionsMonitor<ApplicationOptions>optionsMonitor
    ) =>
{
    var response = new
    {
       OptionsValue= options.Value.ExampleValue,
        SnapshotValue = optionsSnapShot.Value.ExampleValue,
        MonitorValue = optionsMonitor.CurrentValue.ExampleValue
    };
    return Results.Ok(response);
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(s=>s.AllowAnyHeader().AllowAnyOrigin());
app.UseRouting();
var supportedCultures = new[] { "en-US", "ar-EG", "de-DE" };
var localizationOptions = new RequestLocalizationOptions()
    //.SetDefaultCulture(supportedCultures[0])//we used the middleware instead 
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets(new() { KeepAliveInterval=TimeSpan.FromSeconds(30)});
app.Use(async (context, next) =>
{

    context.Response.Headers.Append(CustomHeaderNames.CustomAddName, "custom header value");
    await next();
}
});

app.UseWhen(context => context.Request.Path.Value!.Contains("/middleware"), appBuilder=>
{
    appBuilder.UseMiddleware<ExtractCustomHeaderMiddleware>();
}
app.UseRequestCulture();
    app.MapHub<SignalServer>("Hubs/SignalServer");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseGraphQLAltair();///for interactive query statement 

/*app.UseEndpoints(endpoints =>
endpoints.Map("/ws", async ctx =>
{
      List<WebSocket> _connections = new();


var buffer = new byte[1024 * 4];
    var webSocket = await ctx.WebSockets.AcceptWebSocketAsync();
    _connections.Add(webSocket);

    var result = await webSocket.ReceiveAsync(new(buffer), CancellationToken.None);
    int i = 0;
    while (!result.CloseStatus.HasValue)
    {

        var message = Encoding.UTF8.GetBytes($"message index {i++}");
        foreach (var c in _connections)   await c.SendAsync(new(message, 0, message.Length), result.MessageType, result.EndOfMessage);

        result = await webSocket.ReceiveAsync(new(buffer), CancellationToken.None);

        Console.WriteLine($"Received: {Encoding.UTF8.GetString(buffer[..result.Count])}");

        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);


        _connections.Remove(webSocket);
    } }));
   */
//app.UseGraphQL<Schema>();

/*var config = new ConfigurationBuilder()
.AddJsonFile("appsettings. json")
.Build();

Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(config)
.CreateLogger();

try {

    Log.Information("Application Starting");
    CreateHostBuilder(args).Build().Run();
}
catch (Exception ex)
{

    Log.Fatal(ex, "The application failed to start!");

finally {

    Log.CloseAndFLush(); 
}*/
app.MigrateDatabase();
app.Run();
