using Xunit;
using Moq;
using RunGroopWebApp.Controllers;
using RunGroopWebApp.ViewModels;
using RunGroop.Data.Models.Identity;
using RunGroop.Repository.Interfaces;
using RunGroop.Data.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RunGroop.Data.Data;

public class AccountControllerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<ILocationService> _locationServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        _userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);

        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null, null, null, null);

        _contextMock = new Mock<ApplicationDbContext>();
        _locationServiceMock = new Mock<ILocationService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _controller = new AccountController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _locationServiceMock.Object,
            _unitOfWorkMock.Object);

        // Required to test TempData
        var tempData = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary>();
        _controller.TempData = tempData.Object;
    }

    [Fact]
    public async Task Login_ValidCredentials_ShouldRedirectToRace()
    {
        // Arrange
        var loginViewModel = new LoginViewModel { EmailAddress = "test@example.com", Password = "password" };
        var user = new AppUser { Email = "test@example.com", UserName = "test" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(loginViewModel.EmailAddress))
            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginViewModel.Password))
            .ReturnsAsync(true);
        _signInManagerMock.Setup(x => x.PasswordSignInAsync(user, loginViewModel.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _controller.Login(loginViewModel) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Race", result.ControllerName);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturnViewWithError()
    {
        // Arrange
        var loginViewModel = new LoginViewModel { EmailAddress = "invalid@example.com", Password = "wrongpass" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(loginViewModel.EmailAddress))
            .ReturnsAsync((AppUser)null);

        var tempDataMock = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary>();
        _controller.TempData = tempDataMock.Object;

        // Act
        var result = await _controller.Login(loginViewModel) as ViewResult;

        // Assert
        Assert.NotNull(result);
        tempDataMock.VerifySet(td => td["Error"] = "Wrong credentials. Please try again",Moq.Times.Once);
    }

    [Fact]
    public async Task Register_NewUser_ShouldRedirectToRace()
    {
        // Arrange
        var registerViewModel = new RegisterViewModel { EmailAddress = "newuser@example.com", Password = "password123" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(registerViewModel.EmailAddress))
            .ReturnsAsync((AppUser)null);

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerViewModel.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.Register(registerViewModel) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Race", result.ControllerName);
    }

    [Fact]
    public async Task Register_ExistingEmail_ShouldReturnViewWithError()
    {
        // Arrange
        var registerViewModel = new RegisterViewModel { EmailAddress = "existing@example.com", Password = "password" };
        var existingUser = new AppUser { Email = "existing@example.com", UserName = "existing" };

        _userManagerMock.Setup(x => x.FindByEmailAsync(registerViewModel.EmailAddress))
            .ReturnsAsync(existingUser);

        var tempDataMock = new Mock<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionary>();
        _controller.TempData = tempDataMock.Object;

        // Act
        var result = await _controller.Register(registerViewModel) as ViewResult;

        // Assert
        Assert.NotNull(result);
        tempDataMock.VerifySet(td => td["Error"] = "This email address is already in use", Moq.Times.Once);
    }

    [Fact]
    public async Task Logout_ShouldRedirectToRace()
    {
        // Act
        var result = await _controller.Logout() as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("Race", result.ControllerName);
    }

    [Fact]
    public async Task GetLocation_NullLocation_ShouldReturnNotFound()
    {
        // Act
        var result = await _controller.GetLocation(null) as JsonResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Not found", result.Value);
    }

    //[Fact]
    //public async Task GetLocation_ValidLocation_ShouldReturnJson()
    //{
    //    // Arrange
    //    var location = "New York";
    //    var expectedLocation = "New York, USA";

    //    _locationServiceMock.Setup(x => x.GetLocationSearch(location))
    //        .ReturnsAsync(expectedLocation);

    //    // Act
    //    var result = await _controller.GetLocation(location) as JsonResult;

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(expectedLocation, result.Value);
    //}
}
