using Xunit;
using Moq;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;
using Cloud_Mall.Application.Authentication.Commands.RegisterUser;
using Cloud_Mall.Application.Interfaces;
using Cloud_Mall.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IIdentityRepository> _mockIdentityService;
    private readonly Mock<ITokenGenerator> _mockTokenGenerator;
    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _mockIdentityService = new Mock<IIdentityRepository>();
        _mockTokenGenerator = new Mock<ITokenGenerator>();

        _mockTokenGenerator
            .Setup(t => t.GenerateToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()))
            .Returns("fake-jwt-token");

        _handler = new RegisterUserCommandHandler(
            _mockIdentityService.Object,
            _mockTokenGenerator.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccessAuthenticationResult()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Name = "Khaled",
            Email = "khaled@example.com",
            Password = "Pass@123",
            ConfirmPassword = "Pass@123",
            Role = "User"
        };

        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Name = command.Name,
            Email = command.Email,
            UserName = command.Email
        };

        _mockIdentityService
            .Setup(x => x.FindByEmailAsync(command.Email))
            .ReturnsAsync((ApplicationUser?)null);

        _mockIdentityService
            .Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), command.Password))
            .ReturnsAsync(IdentityResult.Success);

        _mockIdentityService
            .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), command.Role))
            .ReturnsAsync(IdentityResult.Success);

        _mockIdentityService
            .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { command.Role });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // result.Should().NotBeNull();
        // result.Succeeded.Should().BeTrue();
        // result.Token.Should().Be("fake-jwt-token");
        // result.UserId.Should().NotBeNullOrEmpty();
        // result.Errors.Should().BeNullOrEmpty();

        _mockIdentityService.Verify(x => x.FindByEmailAsync(command.Email), Times.Once);
        _mockIdentityService.Verify(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), command.Password), Times.Once);
        _mockIdentityService.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), command.Role), Times.Once);
        _mockTokenGenerator.Verify(x => x.GenerateToken(It.IsAny<ApplicationUser>(), It.IsAny<IList<string>>()), Times.Once);
    }
}
