using Moq;
using TrueCode.UserService.Application.Currencies.Commands.RemoveFavoriteCurrency;
using TrueCode.UserService.Domain.Dao;

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
        var client = clientMock.Object;

        var query = new RemoveFavoriteCurrencyCommand
        {
            UserId = Guid.Empty,
            CurrencyCode = "code",
        };
        var sut = new RemoveFavoriteCurrencyCommandHandler(client);
        
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
        var client = clientMock.Object;

        var query = new RemoveFavoriteCurrencyCommand
        {
            UserId = Guid.NewGuid(),
            CurrencyCode = "",
        };
        var sut = new RemoveFavoriteCurrencyCommandHandler(client);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Код пустой."));
            clientMock.Verify(s => s.RemoveFavoriteCurrencyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        });
    }
}