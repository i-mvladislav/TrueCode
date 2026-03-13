using Moq;
using TrueCode.UserService.Application.Auth.Commands.SignUp;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Tests.Commands;

public class SignUpCommandHandlerTests
{
    [Test]
    public async Task sign_up_returns_error_on_empty_name()
    {
        // Arrange
        var userStorageMock = new Mock<IUserStorage>();
        userStorageMock.Setup(s => s.AddUserAsync(
            It.IsAny<UserEntity>()));

        userStorageMock.Setup(s => s.HasUserAsync(
            It.IsAny<string>()))
            .ReturnsAsync(false);
        
        var query = new SignUpCommand
        {
            Name = "",
            Password = "123",
        };
        var sut = new SignUpCommandHandler(userStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Имя пользователя не может быть пустым."));
        });
    }
    
    [Test]
    public async Task sign_up_returns_error_on_empty_password()
    {
        // Arrange
        var userStorageMock = new Mock<IUserStorage>();
        userStorageMock.Setup(s => s.AddUserAsync(
            It.IsAny<UserEntity>()));

        userStorageMock.Setup(s => s.HasUserAsync(
                It.IsAny<string>()))
            .ReturnsAsync(false);
        
        var query = new SignUpCommand
        {
            Name = "Name",
            Password = "",
        };
        var sut = new SignUpCommandHandler(userStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Пароль не может быть пустым."));
        });
    }
    
    [Test]
    public async Task sign_up_returns_error_if_user_exists()
    {
        // Arrange
        var userStorageMock = new Mock<IUserStorage>();
        userStorageMock.Setup(s => s.AddUserAsync(
            It.IsAny<UserEntity>()));

        userStorageMock.Setup(s => s.HasUserAsync(
                It.IsAny<string>()))
            .ReturnsAsync(true);
        
        var query = new SignUpCommand
        {
            Name = "Name",
            Password = "123",
        };
        var sut = new SignUpCommandHandler(userStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Пользователь с таким именем уже существует."));
        });
    }
}