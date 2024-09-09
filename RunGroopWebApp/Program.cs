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




var builder = WebApplication.CreateBuilder(args);
//add backend services
//glopalization means the application supports many languages and localization means the application adapts to the change of the language

builder.Services.AddHostedService<BackGroundWorkerService>();
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationServices();
//builder.Services.AddConfigurationServices(builder.Configuration);
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections"));
});

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

builder.Services.AddIdentity<AppUser, IdentityRole>().AddDefaultTokenProviders()//.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddLocalization();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddSingleton<ISchema, ClubSchema>();

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
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

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


app.UseRequestCulture();
    app.MapHub<SignalServer>("SignalServer");

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
app.Run();
