using Moq;
using TrueCode.UserService.Application.Currencies.Queries;
using TrueCode.UserService.Domain.Dao;
using TrueCode.UserService.Domain.Entities;

namespace TrueCode.UserService.Tests.Queries;

public class GetFavoriteCurrenciesQueryHandlerTests
{
    [Test]
    public async Task get_favorite_currencies_returns_error_on_empty_user()
    {
        // Arrange
        var currencyStorageMock = new Mock<ICurrencyStorage>();
        currencyStorageMock.Setup(s => s.GetFavoriteCurrenciesAsync(
            It.IsAny<Guid>(), 
            It.IsAny<CancellationToken>()));
        
        var query = new GetFavoriteCurrenciesQuery
        {
            UserId = Guid.Empty,
        };
        var sut = new GetFavoriteCurrenciesQueryHandler(currencyStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Data, Is.Null);
            Assert.That(result.Errors, Has.Count.EqualTo(1));
            Assert.That(result.Errors[0].Message, Is.EqualTo("Некорректный пользователь."));
        });
    }
    
    [Test]
    public async Task get_favorite_currencies_returns_codes()
    {
        // Arrange
        List<FavoriteCurrencyEntity> favorites = [new() { Code = "AUD" }, new() { Code = "AZN" }];
        
        var currencyStorageMock = new Mock<ICurrencyStorage>();
        currencyStorageMock.Setup(s => s.GetFavoriteCurrenciesAsync(
            It.IsAny<Guid>(), 
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(favorites);
        
        var query = new GetFavoriteCurrenciesQuery
        {
            UserId = Guid.NewGuid(),
        };
        var sut = new GetFavoriteCurrenciesQueryHandler(currencyStorageMock.Object);
        
        // Act
        var result = await sut.ExecuteAsync(query);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Data, Is.EquivalentTo(favorites.Select(f => f.Code)));
        });
    }
}