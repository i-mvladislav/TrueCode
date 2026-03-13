using Moq;
using TrueCode.UserService.Application.Currencies.Commands.AddFavoriteCurrency;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Tests.Commands;

public class AddFavoriteCurrencyCommandHandlerTests
{
    [Test]
    public async Task add_favorite_currency_returns_error_on_empty_user()
    {
        // Arrange
        var clientMock = new Mock<ICurrencyStorage>();
        clientMock.Setup(s => s.AddFavoriteCurrencyAsync(
                It.IsAny<FavoriteCurrencyEntity>(),
                It.IsAny<CancellationToken>()));
        var client = clientMock.Object;

        var query = new AddFavoriteCurrencyCommand
        {
            UserId = Guid.Empty,
            CurrencyCode = "code",
        };
        var sut = new AddFavoriteCurrencyCommandHandler(client);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Некорректный пользователь."));
            clientMock.Verify(s => s.AddFavoriteCurrencyAsync(It.IsAny<FavoriteCurrencyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        });
    }
    
    [Test]
    public async Task add_favorite_currency_returns_error_on_empty_currency_code()
    {
        // Arrange
        var clientMock = new Mock<ICurrencyStorage>();
        clientMock.Setup(s => s.AddFavoriteCurrencyAsync(
            It.IsAny<FavoriteCurrencyEntity>(),
            It.IsAny<CancellationToken>()));
        var client = clientMock.Object;

        var query = new AddFavoriteCurrencyCommand
        {
            UserId = Guid.NewGuid(),
            CurrencyCode = "",
        };
        var sut = new AddFavoriteCurrencyCommandHandler(client);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Код пустой."));
            clientMock.Verify(s => s.AddFavoriteCurrencyAsync(It.IsAny<FavoriteCurrencyEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        });
    }
}