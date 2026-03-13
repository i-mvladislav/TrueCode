using Microsoft.Extensions.Configuration;
using Moq;
using TrueCode.UserService.Application.Auth.Commands.SignIn;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;
using TrueCode.UserService.Infrastructure.Auth;

namespace TrueCode.UserService.Tests.Commands;

public class SignInCommandHandlerTests
{
    private SignInCommandHandler PrepareSut()
    {
        var userEntity = new UserEntity
        {
            Name = "Name",
            PasswordHash = "$2a$11$lG71Z.rGco/fiwVVhy32.uODRwsv20B4AZjdIzf8E5iHqOXPlSUSm"
        };

        var jwtConfig = new Dictionary<string, string>()
        {
            ["Jwt:Key"] = "7sA9dF2gHkLp3mN5bV6cX8zQ9wE1rT4yU0iO2pA3S",
            ["Jwt:Issuer"] = "https://localhost:2001",
            ["Jwt:Audiences:0"] = "https://localhost:2001",
            ["Jwt:Audiences:1"] = "https://localhost:3001",
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(jwtConfig)
            .Build();

        var jwtService = new JwtService(configuration);
        var userStorageMock = new Mock<IUserStorage>();
        userStorageMock.Setup(s => s.GetUserByNameAsync(
                It.IsAny<string>()))
            .ReturnsAsync(userEntity);
 
        var sut = new SignInCommandHandler(userStorageMock.Object, jwtService);

        return sut;
    }
    
    [Test]
    public async Task sign_in_returns_error_on_empty_name()
    {
        // Arrange
        var query = new SignInCommand
        {
            Name = "",
            Password = "123",
        };
        var sut = PrepareSut();
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Имя пользователя не может быть пустым."));
        });
    }
    
    [Test]
    public async Task sign_in_returns_error_on_empty_password()
    {
        // Arrange
        var query = new SignInCommand
        {
            Name = "Name",
            Password = "",
        };
        var sut = PrepareSut();
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Errors, Has.Count.EqualTo(2));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Пароль не может быть пустым."));
        });
    }
    
    [Test]
    public async Task sign_in_returns_error_if_password_not_match_the_hash()
    {
        // Arrange
        var query = new SignInCommand
        {
            Name = "Name",
            Password = "1233",
        };
        var sut = PrepareSut();
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Неверное имя или пароль."));
        });
    }
}