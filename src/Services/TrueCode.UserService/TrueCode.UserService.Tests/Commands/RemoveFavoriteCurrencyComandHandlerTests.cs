using Moq;
using TrueCode.Core.Users;
using TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Tests.Commands;

public class RemoveFavoriteCurrencyComandHandlerTests
{
    [Test]
    public async Task remove_favorite_currency_returns_error_on_empty_user()
    {
        // Arrange
        var clientMock = new Mock<ICurrencyStorage>();
        clientMock.Setup(s => s.RemoveFavoriteCurrencyAsync(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
        clientMock.Setup(s => s.GetFavoriteCurrencyAsync(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FavoriteCurrencyEntity());
    
        var userContextMock = new Mock<ICurrentUserContext>();
        userContextMock.SetupGet(s => s.IsAuthenticated).Returns(true);
        userContextMock.SetupGet(s => s.UserId).Returns(Guid.Empty.ToString());
        userContextMock.SetupGet(s => s.Authorization).Returns("token");
        
        var query = new RemoveFavoriteCurrencyCommand
        {
            Name = "code",
        };
        var sut = new RemoveFavoriteCurrencyCommandHandler(clientMock.Object, userContextMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Пользователь не найден."));
            clientMock.Verify(s => s.RemoveFavoriteCurrencyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        });
    }
    
    [Test]
    public async Task remove_favorite_currency_returns_error_on_empty_currency_code()
    {
        // Arrange
        var clientMock = new Mock<ICurrencyStorage>();
        clientMock.Setup(s => s.RemoveFavoriteCurrencyAsync(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
        clientMock.Setup(s => s.GetFavoriteCurrencyAsync(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FavoriteCurrencyEntity());
        
        var userContextMock = new Mock<ICurrentUserContext>();
        userContextMock.SetupGet(s => s.IsAuthenticated).Returns(true);
        userContextMock.SetupGet(s => s.UserId).Returns(Guid.NewGuid().ToString());
        userContextMock.SetupGet(s => s.Authorization).Returns("token");
        
        var query = new RemoveFavoriteCurrencyCommand
        {
            Name = "",
        };
        var sut = new RemoveFavoriteCurrencyCommandHandler(clientMock.Object, userContextMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Имя валюты пустое."));
            clientMock.Verify(s => s.RemoveFavoriteCurrencyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        });
    }
    
    [Test]
    public async Task remove_favorite_currency_returns_error_not_found_favorite_currency()
    {
        // Arrange
        var clientMock = new Mock<ICurrencyStorage>();
        clientMock.Setup(s => s.RemoveFavoriteCurrencyAsync(
            It.IsAny<Guid>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
        
        var userContextMock = new Mock<ICurrentUserContext>();
        userContextMock.SetupGet(s => s.IsAuthenticated).Returns(true);
        userContextMock.SetupGet(s => s.UserId).Returns(Guid.NewGuid().ToString());
        userContextMock.SetupGet(s => s.Authorization).Returns("token");
        
        var query = new RemoveFavoriteCurrencyCommand
        {
            Name = "Name",
        };
        var sut = new RemoveFavoriteCurrencyCommandHandler(clientMock.Object, userContextMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Не найдена любимая валюта."));
            clientMock.Verify(s => s.RemoveFavoriteCurrencyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        });
    }
}